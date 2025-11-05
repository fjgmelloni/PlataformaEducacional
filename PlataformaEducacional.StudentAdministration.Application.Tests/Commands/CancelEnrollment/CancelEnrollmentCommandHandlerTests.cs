using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CancelEnrollment;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.CancelEnrollment
{
    public class CancelEnrollmentCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly CancelEnrollmentCommandHandler _handler;

        public CancelEnrollmentCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CancelEnrollmentCommandHandler>();
        }
       
        [Fact(DisplayName = "Should deactivate enrollment and commit successfully")]
        [Trait("Category", "CancelEnrollmentCommandHandler")]
        public async Task Handle_ValidCommand_ExistingEnrollment_ShouldReturnTrueAndDeactivate()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var command = new CancelEnrollmentCommand(enrollmentId, studentId);

            var enrollment = new Enrollment(Guid.NewGuid(), "Course Test", 10, 100);
            enrollment.AssignStudent(studentId);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithStudentById(enrollmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(enrollment);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(EnrollmentStatus.PendingPayment, enrollment.EnrollmentStatus);
            _mocker.GetMock<IStudentRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()), Times.Never);
        }


        [Fact(DisplayName = "Should return false and notify when enrollment does not exist")]
        [Trait("Category", "CancelEnrollmentCommandHandler")]
        public async Task Handle_EnrollmentNotFound_ShouldReturnFalseAndNotify()
        {
            // Arrange
            var command = new CancelEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid());

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithStudentById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Enrollment)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.PublishNotificationAsync(It.Is<DomainNotification>(n => n.Value == "Enrollment not found.")),
                Times.Once);
            _mocker.GetMock<IStudentRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        [Fact(DisplayName = "Should return false and publish validation errors when command invalid")]
        [Trait("Category", "CancelEnrollmentCommandHandler")]
        public async Task Handle_InvalidCommand_ShouldReturnFalseAndNotify()
        {
            // Arrange
            var command = new CancelEnrollmentCommand(Guid.Empty, Guid.NewGuid());
            command.IsValid(); // popula erros

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()),
                Times.AtLeastOnce);
            _mocker.GetMock<IStudentRepository>().Verify(
                r => r.GetEnrollmentWithStudentById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
