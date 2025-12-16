using Bogus;
using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddCourse;

namespace PlataformaEducacional.ContentManagement.Application.Tests.Features.Courses.Commands.AddCourse
{
    public class AddCourseCommandTests
    {

        [Fact(DisplayName = "Add Course Command should be valid when data is correct")]
        [Trait("Category", "Content Management - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeValid_WhenDataIsCorrect()
        {
            // Arrange
            var command = new AddCourseCommand(
                "C# Course",
                "Course syllabus description",
                5,
                500,
                true
            );

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Should be invalid when name is empty")]
        [Trait("Category", "Content Management - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenNameIsEmpty()
        {
            var command = new AddCourseCommand(
                "",
                "Course syllabus description",
                5,
                500,
                true
            );

            var result = command.IsValid();

            Assert.False(result);
        }

        [Fact(DisplayName = "Should be invalid when syllabus description is empty")]
        [Trait("Category", "Content Management - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenSyllabusDescriptionIsEmpty()
        {
            var command = new AddCourseCommand(
                "C# Course",
                "",
                5,
                500,
                true
            );

            var result = command.IsValid();

            Assert.False(result);
        }

        [Fact(DisplayName = "Should be invalid when syllabus workload is less than or equal to zero")]
        [Trait("Category", "Content Management - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenSyllabusWorkloadIsInvalid()
        {
            var command = new AddCourseCommand(
                "C# Course",
                "Course syllabus description",
                0,
                500,
                true
            );

            var result = command.IsValid();

            Assert.False(result);
        }

        [Fact(DisplayName = "Should be invalid when price is negative")]
        [Trait("Category", "Content Management - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenPriceIsNegative()
        {
            var command = new AddCourseCommand(
                "C# Course",
                "Course syllabus description",
                5,
                -1,
                true
            );

            var result = command.IsValid();

            Assert.False(result);
        }
    }
}
