using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddLesson;

namespace PlataformaEducacional.ContentManagement.Application.Tests.Commands
{
    public class AddLessonCommandTest
    {
        [Fact(DisplayName = "Add Lesson Command Valid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeValid_WhenDataIsCorrect()
        {
            // Arrange
            var command = new AddLessonCommand("Lesson 1", "Lesson content", 1, "Material", Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Should be invalid when the title is invalid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenTitleIsEmpty()
        {
            // Arrange
            var command = new AddLessonCommand("", "Lesson content", 1, "Material", Guid.NewGuid());

            // Act
            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e == "Lesson title is required.");
        }

        [Fact(DisplayName = "Should be invalid when content is invalid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenContentIsEmpty()
        {
            // Arrange
            var command = new AddLessonCommand("Lesson 1", "  ", 1, "Material", Guid.NewGuid());

            // Act
            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e == "Content is required.");
        }

        [Fact(DisplayName = "Should be invalid when the order is invalid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenOrderIsLessThanOrEqualZero()
        {
            // Arrange
            var command = new AddLessonCommand("Lesson 1", "Lesson content", 0, "Material", Guid.NewGuid());

            // Act
            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e == "The lesson order must be greater than 0.");
        }

        [Fact(DisplayName = "Should be invalid when the course is invalid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenCourseIsInvalid()
        {
            // Arrange
            var command = new AddLessonCommand("Lesson 1", "Lesson content", 1, "Material", Guid.Empty);

            // Act
            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains( "Course is required.",command.ValidationResult.Errors);
        }
    }
}
