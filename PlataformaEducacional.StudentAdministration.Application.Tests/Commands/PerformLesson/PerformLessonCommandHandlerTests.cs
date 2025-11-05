using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PerformLesson;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.PerformLesson
{
    public class PerformLessonCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly PerformLessonCommandHandler _handler;
        private readonly Guid _enrollmentId = Guid.NewGuid();
        private readonly Guid _lessonId = Guid.NewGuid();

        public PerformLessonCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<PerformLessonCommandHandler>();
        }

        [Fact(DisplayName = "Should record lesson and commit successfully")]
        [Trait("Category", "PerformLessonCommandHandler")]
        public async Task Handle_ShouldRecordLessonAndCommit()
        {
            // Arrange
            var command = new PerformLessonCommand(_enrollmentId, _lessonId);
            var enrollment = new Enrollment(Guid.NewGuid(), "C# Course", 10, 500);
            enrollment.Activate();

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithProgressById(_enrollmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(enrollment);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Single(enrollment.LessonProgresses);
        }

        [Fact(DisplayName = "Should notify and return false when enrollment not found")]
        [Trait("Category", "PerformLessonCommandHandler")]
        public async Task Handle_EnrollmentNotFound_ShouldReturnFalseAndNotify()
        {
            var command = new PerformLessonCommand(_enrollmentId, _lessonId);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithProgressById(_enrollmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Enrollment)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>()
                .Verify(m => m.PublishNotificationAsync(It.Is<DomainNotification>(n => n.Value == "Matrícula não encontrada.")),
                Times.Once);
        }

        [Fact(DisplayName = "Should notify and return false when command invalid")]
        [Trait("Category", "PerformLessonCommandHandler")]
        public async Task Handle_InvalidCommand_ShouldReturnFalseAndNotify()
        {
            // Arrange
            var command = new PerformLessonCommand(Guid.Empty, Guid.Empty);
            command.IsValid(); // fill validation errors

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>()
                .Verify(m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()), Times.AtLeast(1));

            _mocker.GetMock<IStudentRepository>()
                .Verify(r => r.GetEnrollmentWithProgressById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
