using Moq;
using Moq.AutoMock;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddCourse;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.UpdateCourse;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddLesson;
using PlataformaEducacional.ContentManagement.Domain.Courses;
using PlataformaEducacional.ContentManagement.Domain.Lessons;
using PlataformaEducacional.ContentManagement.Domain.ValueObjects;
using PlataformaEducacao.ContentManagement.Application.Features.Courses.Commands;

namespace PlataformaEducacional.ContentManagement.Application.Tests.Features.Courses.Commands
{
    public class CourseCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly CourseCommandHandler _handler;

        public CourseCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CourseCommandHandler>();
        }
        [Fact(DisplayName = "Add course should return false when command is invalid")]
        [Trait("Category", "Content Management - CourseCommandHandler")]
        public async Task AddCourse_ShouldReturnFalse_WhenCommandIsInvalid()
        {
            var command = new AddCourseCommand(
                "",     
                "",    
                0,     
                -1,    
                true
            );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
        }





        [Fact(DisplayName = "Add course should return false when course name already exists")]
        [Trait("Category", "Content Management - CourseCommandHandler")]
        public async Task AddCourse_ShouldReturnFalse_WhenCourseNameAlreadyExists()
        {
            var course = new Course(
                "C# Course",
                new Syllabus("Description", 50),
                500,
                true
            );

            var command = new AddCourseCommand(
                "C# Course",
                "Content",
                20,
                500,
                true
            );

            _mocker
                .GetMock<ICourseRepository>()
                .Setup(r => r.GetByNameAsync(command.Name, CancellationToken.None))
                .ReturnsAsync(course);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mocker
                .GetMock<IMediatorHandler>()
                .Verify(
                    m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()),
                    Times.Once
                );
        }

        [Fact(DisplayName = "Add course should execute successfully when command is valid")]
        [Trait("Category", "Content Management - CourseCommandHandler")]
        public async Task AddCourse_CommandValid_ShouldExecuteSuccessfully()
        {
            var command = new AddCourseCommand(
                "C# Course",
                "Content",
                20,
                500,
                true
            );

            _mocker
                .GetMock<ICourseRepository>()
                .Setup(r => r.GetByNameAsync(command.Name, CancellationToken.None))
                .ReturnsAsync((Course?)null);

            _mocker
                .GetMock<ICourseRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);

            _mocker.GetMock<ICourseRepository>()
                .Verify(r => r.GetByNameAsync(command.Name, CancellationToken.None), Times.Once);

            _mocker.GetMock<ICourseRepository>()
                .Verify(r => r.AddAsync(It.IsAny<Course>(), CancellationToken.None), Times.Once);

            _mocker.GetMock<ICourseRepository>()
                .Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Update course should return false when course does not exist")]
        [Trait("Category", "Content Management - CourseCommandHandler")]
        public async Task UpdateCourse_ShouldReturnFalse_WhenCourseDoesNotExist()
        {
            var command = new UpdateCourseCommand(
                Guid.NewGuid(),
                "C# Language Course",
                "Course content",
                20,
                500,
                true
            );

            _mocker
                .GetMock<ICourseRepository>()
                .Setup(r => r.GetByIdAsync(command.CourseId, CancellationToken.None))
                .ReturnsAsync((Course?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>()
                .Verify(m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()), Times.Once);
        }

        [Fact(DisplayName = "Update course should execute successfully when command is valid")]
        [Trait("Category", "Content Management - CourseCommandHandler")]
        public async Task UpdateCourse_CommandValid_ShouldExecuteSuccessfully()
        {
            var course = new Course(
                "C# Course",
                new Syllabus("Syllabus", 20),
                500,
                true
            );

            var command = new UpdateCourseCommand(
                course.Id,
                "C# Language Course",
                "Updated syllabus",
                25,
                600,
                true
            );

            _mocker
                .GetMock<ICourseRepository>()
                .Setup(r => r.GetByIdAsync(command.CourseId, CancellationToken.None))
                .ReturnsAsync(course);

            _mocker
                .GetMock<ICourseRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);

            _mocker.GetMock<ICourseRepository>()
                .Verify(r => r.UpdateAsync(It.IsAny<Course>(), CancellationToken.None), Times.Once);

            _mocker.GetMock<ICourseRepository>()
                .Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add lesson should execute successfully when first lesson is valid")]
        [Trait("Category", "Content Management - CourseCommandHandler")]
        public async Task AddLesson_FirstLesson_ShouldExecuteSuccessfully()
        {
            var course = new Course(
                "C# Course",
                new Syllabus("Syllabus", 20),
                500,
                true
            );

            var command = new AddLessonCommand(
                "Lesson 1",
                "Lesson content",
                1,
                "Material",
                course.Id
            );

            _mocker
                .GetMock<ICourseRepository>()
                .Setup(r => r.GetWithLessonsByIdAsync(command.CourseId, CancellationToken.None))
                .ReturnsAsync(course);

            _mocker
                .GetMock<ICourseRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            Assert.Single(course.Lessons);
        }
    }
}
