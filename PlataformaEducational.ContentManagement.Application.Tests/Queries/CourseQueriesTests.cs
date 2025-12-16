using Moq;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Queries.ViewModels;
using PlataformaEducacional.ContentManagement.Domain.Courses;
using PlataformaEducacional.ContentManagement.Domain.Lessons;
using PlataformaEducacional.ContentManagement.Domain.ValueObjects;

namespace PlataformaEducacional.ContentManagement.Application.Tests.Features.Courses.Queries
{
    public class CourseQueriesTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly CourseQueries _queries;

        public CourseQueriesTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _queries = new CourseQueries(_courseRepositoryMock.Object);
        }

        [Fact(DisplayName = "Should return null when course is not found")]
        [Trait("Category", "Content Management - CourseQueries")]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCourseIsNotFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            _courseRepositoryMock
                .Setup(r => r.GetByIdAsync(courseId, default))
                .ReturnsAsync((Course?)null);

            // Act
            var result = await _queries.GetByIdAsync(courseId, default);

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Should return course view model when course exists")]
        [Trait("Category", "Content Management - CourseQueries")]
        public async Task GetByIdAsync_ShouldReturnCourseViewModel_WhenCourseExists()
        {
            // Arrange
            var course = new Course(
                "C# Course",
                new Syllabus("Syllabus description", 200),
                500,
                true
            );

            _courseRepositoryMock
                .Setup(r => r.GetByIdAsync(course.Id, default))
                .ReturnsAsync(course);

            // Act
            var result = await _queries.GetByIdAsync(course.Id, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(course.Id, result!.Id);
            Assert.Equal(course.Name, result.Name);
        }

        [Fact(DisplayName = "Should return all courses")]
        [Trait("Category", "Content Management - CourseQueries")]
        public async Task GetAllAsync_ShouldReturnAllCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course("C# Course", new Syllabus("Syllabus", 200), 500, true),
                new Course("Angular Course", new Syllabus("Syllabus", 150), 450, true)
            };

            _courseRepositoryMock
                .Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(courses);

            // Act
            var result = await _queries.GetAllAsync(default);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact(DisplayName = "Should return only available courses with lessons")]
        public async Task GetAvailableWithLessonsAsync_ShouldReturnOnlyAvailableCourses()
        {
            var courses = new List<Course>
    {
        new Course("C# Course", new Syllabus("Syllabus", 200), 500, true),
        new Course("Angular Course", new Syllabus("Syllabus", 150), 450, false)
    };

            _courseRepositoryMock
                .Setup(r => r.GetAvailableWithLessonsAsync(default))
                .ReturnsAsync(courses.Where(c => c.IsAvailable).ToList());

            var result = await _queries.GetAvailableWithLessonsAsync(default);

            Assert.Single(result);
        }


        [Fact(DisplayName = "Should return all lessons for a course")]
        [Trait("Category", "Content Management - CourseQueries")]
        public async Task GetLessonsByCourseIdAsync_ShouldReturnLessons()
        {
            // Arrange
            var course = new Course(
                "C# Course",
                new Syllabus("Syllabus", 200),
                500,
                true
            );

            course.AddLesson(new Lesson("Lesson 1", "Lesson content 1", 1, "Material 1"));
            course.AddLesson(new Lesson("Lesson 2", "Lesson content 2", 2, "Material 2"));

            _courseRepositoryMock
                .Setup(r => r.GetWithLessonsByIdAsync(course.Id, default))
                .ReturnsAsync(course);

            // Act
            var result = await _queries.GetLessonsByCourseIdAsync(course.Id, default);

            // Assert
            Assert.Equal(course.Lessons.Count, result.Count);
        }
    }
}
