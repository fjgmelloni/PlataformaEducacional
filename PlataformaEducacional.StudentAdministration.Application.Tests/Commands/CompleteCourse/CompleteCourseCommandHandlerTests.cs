using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteCourse;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.CompleteCourse
{
    public class CompleteCourseCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly CompleteCourseCommandHandler _handler;

        public CompleteCourseCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CompleteCourseCommandHandler>();
        }

        [Fact(DisplayName = "Should complete course and commit successfully")]
        public async Task Handle_ValidEnrollment_ShouldCompleteCourse()
        {
            // Arrange
            var enrollment = new Enrollment(Guid.NewGuid(), "Curso Teste", 1, 200);
            enrollment.AssignStudent(Guid.NewGuid());
            enrollment.Activate();
            enrollment.RecordLesson(new LessonProgress(Guid.NewGuid())); // progresso 100%

            var command = new CompleteCourseCommand(enrollment.Id, enrollment.StudentId);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithProgressById(enrollment.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(enrollment);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(CourseStatus.Completed, enrollment.LearningHistory.CourseStatus);
            _mocker.GetMock<IMediatorHandler>().Verify(n => n.PublishNotificationAsync(It.IsAny<DomainNotification>()), Times.Never);
        }

        [Fact(DisplayName = "Should notify and return false when enrollment not found")]
        public async Task Handle_EnrollmentNotFound_ShouldNotifyAndReturnFalse()
        {
            // Arrange
            var command = new CompleteCourseCommand(Guid.NewGuid(), Guid.NewGuid());

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetEnrollmentWithProgressById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Enrollment)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.PublishNotificationAsync(It.Is<DomainNotification>(n => n.Value == "Matrícula não encontrada.")),
                Times.Once);
        }
    }
}
