using PlataformaEducacional.ContentManagement.Application.Features.Courses.Commands.AddLesson;

namespace PlataformaEducacional.ContentManagement.Application.Tests.Commands
{
    public class AddLessonCommandTest
    {
        [Fact(DisplayName = "Add Lesson Command Valid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeValid_WhenDataIsCorrect()
        {
            var command = new AddLessonCommand(
                "Lesson 1",
                "Lesson content",
                1,
                "Material",
                Guid.NewGuid()
            );

            var result = command.IsValid();

            Assert.True(result);
        }

        [Fact(DisplayName = "Should be invalid when the title is invalid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenTitleIsEmpty()
        {
            var command = new AddLessonCommand(
                "",
                "Lesson content",
                1,
                "Material",
                Guid.NewGuid()
            );

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(
                "O título da aula é obrigatório.",
                command.ValidationResult.Errors
            );
        }

        [Fact(DisplayName = "Should be invalid when content is invalid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenContentIsEmpty()
        {
            var command = new AddLessonCommand(
                "Lesson 1",
                "   ",
                1,
                "Material",
                Guid.NewGuid()
            );

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(
                "O conteúdo da aula é obrigatório.",
                command.ValidationResult.Errors
            );
        }

        [Fact(DisplayName = "Should be invalid when the order is invalid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenOrderIsLessThanOrEqualZero()
        {
            var command = new AddLessonCommand(
                "Lesson 1",
                "Lesson content",
                0,
                "Material",
                Guid.NewGuid()
            );

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(
                "A ordem da aula deve ser maior que 0.",
                command.ValidationResult.Errors
            );
        }

        [Fact(DisplayName = "Should be invalid when the course is invalid")]
        [Trait("Category", "Content Management - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenCourseIsInvalid()
        {
            var command = new AddLessonCommand(
                "Lesson 1",
                "Lesson content",
                1,
                "Material",
                Guid.Empty
            );

            var result = command.IsValid();

            Assert.False(result);
            Assert.Contains(
                "O identificador do curso é obrigatório.",
                command.ValidationResult.Errors
            );
        }
    }
}
