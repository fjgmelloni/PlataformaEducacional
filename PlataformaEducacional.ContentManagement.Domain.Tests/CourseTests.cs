using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.ContentManagement.Domain.Courses;
using PlataformaEducacional.ContentManagement.Domain.Lessons;
using PlataformaEducacional.ContentManagement.Domain.ValueObjects;

namespace PlataformaEducacional.ContentManagement.Domain.Tests.Courses
{
    public class CourseTests
    {
        [Fact(DisplayName = "Should throw exception when course name is empty")]
        [Trait("Category", "Content Management - Course")]
        public void CreateCourse_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            var invalidName = string.Empty;
            var syllabus = new Syllabus("Syllabus description", 40);

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Course(invalidName, syllabus, 500, true)
            );

            // Assert
            Assert.Equal("The course name is required.", ex.Message);
        }

        [Fact(DisplayName = "Should throw exception when course price is invalid")]
        [Trait("Category", "Content Management - Course")]
        public void CreateCourse_ShouldThrowException_WhenPriceIsLessOrEqualZero()
        {
            // Arrange
            var syllabus = new Syllabus("Syllabus description", 40);

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Course("C# Course", syllabus, 0, true)
            );

            // Assert
            Assert.Equal("The course price must be greater than 0.", ex.Message);
        }

        [Fact(DisplayName = "Should throw exception when course name exceeds max length")]
        [Trait("Category", "Content Management - Course")]
        public void CreateCourse_ShouldThrowException_WhenNameIsTooLong()
        {
            // Arrange
            var longName = new string('A', 256);
            var syllabus = new Syllabus("Syllabus description", 40);

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Course(longName, syllabus, 500, true)
            );

            // Assert
            Assert.Equal("The course name must be at most 255 characters.", ex.Message);
        }

        [Fact(DisplayName = "Should throw exception when syllabus is null")]
        [Trait("Category", "Content Management - Course")]
        public void CreateCourse_ShouldThrowException_WhenSyllabusIsNull()
        {
            // Act
            var ex = Assert.Throws<DomainException>(() =>
                new Course("C# Course", null!, 500, true)
            );

            // Assert
            Assert.Equal("The syllabus is required.", ex.Message);
        }

        [Fact(DisplayName = "Should create course successfully when data is valid")]
        [Trait("Category", "Content Management - Course")]
        public void CreateCourse_ShouldCreateSuccessfully_WhenValid()
        {
            // Arrange
            var syllabus = new Syllabus("Syllabus description", 40);

            // Act
            var course = new Course("C# Course", syllabus, 500, true);

            // Assert
            Assert.Equal("C# Course", course.Name);
            Assert.Equal(500, course.Price);
            Assert.True(course.IsAvailable);
            Assert.Equal(syllabus, course.Syllabus);
            Assert.Empty(course.Lessons);
        }

        [Fact(DisplayName = "Should update course name successfully")]
        [Trait("Category", "Content Management - Course")]
        public void UpdateName_ShouldUpdateSuccessfully()
        {
            // Arrange
            var course = new Course(
                "Old Name",
                new Syllabus("Syllabus", 40),
                500,
                true
            );

            // Act
            course.UpdateName("New Name");

            // Assert
            Assert.Equal("New Name", course.Name);
        }

        [Fact(DisplayName = "Should update course price successfully")]
        [Trait("Category", "Content Management - Course")]
        public void UpdatePrice_ShouldUpdateSuccessfully()
        {
            // Arrange
            var course = new Course(
                "C# Course",
                new Syllabus("Syllabus", 40),
                500,
                true
            );

            // Act
            course.UpdatePrice(600);

            // Assert
            Assert.Equal(600, course.Price);
        }

        [Fact(DisplayName = "Should add lesson to course successfully")]
        [Trait("Category", "Content Management - Course")]
        public void AddLesson_ShouldAddSuccessfully()
        {
            // Arrange
            var course = new Course(
                "C# Course",
                new Syllabus("Syllabus", 40),
                500,
                true
            );

            var lesson = new Lesson("Lesson 1", "Lesson content", 1, null);

            // Act
            course.AddLesson(lesson);

            // Assert
            Assert.Single(course.Lessons);
            Assert.Equal(course.Id, lesson.CourseId);
        }

        [Fact(DisplayName = "Should throw exception when adding duplicate lesson")]
        [Trait("Category", "Content Management - Course")]
        public void AddLesson_ShouldThrowException_WhenLessonAlreadyExists()
        {
            // Arrange
            var course = new Course(
                "C# Course",
                new Syllabus("Syllabus", 40),
                500,
                true
            );

            var lesson = new Lesson("Lesson 1", "Lesson content", 1, null);
            course.AddLesson(lesson);

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                course.AddLesson(new Lesson("Lesson 1", "Other content", 2, null))
            );

            // Assert
            Assert.Equal("Lesson already associated with this course.", ex.Message);
        }

        [Fact(DisplayName = "Should change course availability")]
        [Trait("Category", "Content Management - Course")]
        public void ChangeAvailability_ShouldWorkCorrectly()
        {
            // Arrange
            var course = new Course(
                "C# Course",
                new Syllabus("Syllabus", 40),
                500,
                false
            );

            // Act
            course.MakeAvailable();

            // Assert
            Assert.True(course.IsAvailable);

            // Act
            course.MakeUnavailable();

            // Assert
            Assert.False(course.IsAvailable);
        }
    }
}
