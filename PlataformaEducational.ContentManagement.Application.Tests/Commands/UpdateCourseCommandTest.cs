using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.UpdateCourse;

namespace PlataformaEducacional.ContentManagement.Application.Tests.Features.Courses.Commands.UpdateCourse
{
    public class UpdateCourseCommandTests
    {
        [Fact(DisplayName = "Update Course Command should be valid when data is correct")]
        [Trait("Category", "Content Management - UpdateCourseCommand")]
        public void UpdateCourseCommand_ShouldBeValid_WhenDataIsCorrect()
        {
            var command = new UpdateCourseCommand(
                Guid.NewGuid(),
                "C# Course",
                "Course syllabus description",
                5,
                500,
                true
            );

            var result = command.IsValid();

            Assert.True(result);
        }

        [Fact(DisplayName = "Should be invalid when course id is empty")]
        [Trait("Category", "Content Management - UpdateCourseCommand")]
        public void UpdateCourseCommand_ShouldBeInvalid_WhenCourseIdIsEmpty()
        {
            var command = new UpdateCourseCommand(
                Guid.Empty,
                "C# Course",
                "Course syllabus description",
                5,
                500,
                true
            );

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(
                "O identificador do curso é obrigatório.",
                command.ValidationResult.Errors
            );
        }

        [Fact(DisplayName = "Should be invalid when name is empty")]
        [Trait("Category", "Content Management - UpdateCourseCommand")]
        public void UpdateCourseCommand_ShouldBeInvalid_WhenNameIsEmpty()
        {
            var command = new UpdateCourseCommand(
                Guid.NewGuid(),
                "",
                "Course syllabus description",
                5,
                500,
                true
            );

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(
                "O nome do curso é obrigatório.",
                command.ValidationResult.Errors
            );
        }

        [Fact(DisplayName = "Should be invalid when syllabus description is empty")]
        [Trait("Category", "Content Management - UpdateCourseCommand")]
        public void UpdateCourseCommand_ShouldBeInvalid_WhenSyllabusDescriptionIsEmpty()
        {
            var command = new UpdateCourseCommand(
                Guid.NewGuid(),
                "C# Course",
                "",
                5,
                500,
                true
            );

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(
                "A descrição do curso é obrigatória.",
                command.ValidationResult.Errors
            );
        }

        [Fact(DisplayName = "Should be invalid when syllabus workload is less than or equal to zero")]
        [Trait("Category", "Content Management - UpdateCourseCommand")]
        public void UpdateCourseCommand_ShouldBeInvalid_WhenSyllabusWorkloadIsInvalid()
        {
            var command = new UpdateCourseCommand(
                Guid.NewGuid(),
                "C# Course",
                "Course syllabus description",
                0,
                500,
                true
            );

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(
                "A carga horária deve ser maior que 0.",
                command.ValidationResult.Errors
            );
        }

        [Fact(DisplayName = "Should be invalid when price is less than or equal to zero")]
        [Trait("Category", "Content Management - UpdateCourseCommand")]
        public void UpdateCourseCommand_ShouldBeInvalid_WhenPriceIsInvalid()
        {
            var command = new UpdateCourseCommand(
                Guid.NewGuid(),
                "C# Course",
                "Course syllabus description",
                5,
                0,
                true
            );

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(
                "O preço do curso deve ser maior que 0.",
                command.ValidationResult.Errors
            );
        }
    }
}
