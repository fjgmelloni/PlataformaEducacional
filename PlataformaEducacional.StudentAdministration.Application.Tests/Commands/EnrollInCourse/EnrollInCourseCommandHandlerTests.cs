using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.EnrollInCourse;
using PlataformaEducacional.StudentAdministration.Domain;
using PlataformaEducacional.StudentAdministration.Domain.Repositories;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.EnrollInCourse
{
    public class EnrollInCourseCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly EnrollInCourseCommandHandler _handler;

        public EnrollInCourseCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<EnrollInCourseCommandHandler>();
        }

        [Fact(DisplayName = "Should return false and publish validation errors when command invalid")]
        [Trait("Category", "EnrollInCourseCommandHandler")]
        public async Task Should_Return_False_When_Command_Invalid()
        {
            var command = new EnrollInCourseCommand(Guid.Empty, Guid.NewGuid(), "C# Course", 5, 500);
            command.IsValid(); // populates ValidationResult

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublishNotificationAsync(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Should return false and notify when student not found")]
        [Trait("Category", "EnrollInCourseCommandHandler")]
        public async Task Should_Return_False_When_Student_Not_Found()
        {
            var command = new EnrollInCourseCommand(Guid.NewGuid(), Guid.NewGuid(), "C# Course", 5, 500);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetWithEnrollmentsById(command.StudentId, default))
                .ReturnsAsync((Student)null!);

            var result = await _handler.Handle(command, default);

            Assert.False(result);

            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.PublishNotificationAsync(It.Is<DomainNotification>(n => n.Value == "Aluno não encontrado.")),
                Times.Once);
        }

        [Fact(DisplayName = "Should create enrollment and commit successfully")]
        [Trait("Category", "EnrollInCourseCommandHandler")]
        public async Task Should_Enroll_And_Commit()
        {
            var student = new Student(Guid.NewGuid(), "Felício");
            var command = new EnrollInCourseCommand(student.Id, Guid.NewGuid(), "C# Course", 10, 500);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.GetWithEnrollmentsById(student.Id, default))
                .ReturnsAsync(student);

            _mocker.GetMock<IStudentRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, default);

            Assert.True(result);
            _mocker.GetMock<IStudentRepository>().Verify(r =>
                r.EnrollStudentInCourse(It.IsAny<Enrollment>(), default), Times.Once);

            _mocker.GetMock<IMediatorHandler>().Verify(
                m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()), Times.Never);
        }
    }
}
