using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Data;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteEnrollment;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.CompleteEnrollment
{
    public class CompleteEnrollmentCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly CompleteEnrollmentCommandHandler _handler;

        public CompleteEnrollmentCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CompleteEnrollmentCommandHandler>();
        }

        [Fact(DisplayName = "Should successfully complete enrollment and commit")]
        [Trait("Category", "CompleteEnrollmentCommandHandler")]
        public async Task Handle_ValidCommand_ExistingEnrollment_ShouldReturnTrue()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var command = new CompleteEnrollmentCommand(enrollmentId, studentId);
            var enrollment = new Enrollment(Guid.NewGuid(), "C# Course", 10, 500);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithStudentById(enrollmentId, default))
                .ReturnsAsync(enrollment);

            _mocker.GetMock<IUnitOfWork>()
                .Setup(u => u.Commit())
                .ReturnsAsync(true);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(_mocker.GetMock<IUnitOfWork>().Object);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert            
            Assert.True(result);
            Assert.Equal(EnrollmentStatus.Active, enrollment.EnrollmentStatus);
        }

        [Fact(DisplayName = "Should notify and return false when enrollment not found")]
        [Trait("Category", "CompleteEnrollmentCommandHandler")]
        public async Task Handle_EnrollmentNotFound_ShouldReturnFalseAndNotify()
        {
            // Arrange
            var command = new CompleteEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid());

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithStudentById(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Enrollment)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);

            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.PublishNotificationAsync(
                    It.Is<DomainNotification>(n =>
                        n.Key == "Enrollment" &&
                        n.Value == "Matrícula não encontrada.")
                ),
                Times.Once
            );
        }


        [Fact(DisplayName = "Should return false and publish validation errors when command invalid")]
        [Trait("Category", "CompleteEnrollmentCommandHandler")]
        public async Task Handle_InvalidCommand_ShouldNotifyValidationErrors()
        {
            // Arrange
            var command = new CompleteEnrollmentCommand(Guid.Empty, Guid.Empty);

            // Em vez de recriar o ValidationResult:
            command.ValidationResult.AddError("Enrollment is required.");
            command.ValidationResult.AddError("Student is required.");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()),
                Times.Exactly(2));
        }

    }
}
