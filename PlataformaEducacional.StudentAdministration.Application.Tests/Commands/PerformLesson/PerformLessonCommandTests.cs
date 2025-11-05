using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.PerformLesson;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.PerformLesson
{
    public class PerformLessonCommandTests
    {
        [Fact(DisplayName = "Should be invalid when LessonId is empty")]
        [Trait("Category", "PerformLessonCommand")]
        public void LessonId_Empty_ShouldBeInvalid()
        {
            // Arrange
            var command = new PerformLessonCommand(Guid.NewGuid(), Guid.Empty);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("O ID da aula é obrigatório.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be invalid when EnrollmentId is empty")]
        [Trait("Category", "PerformLessonCommand")]
        public void EnrollmentId_Empty_ShouldBeInvalid()
        {
            // Arrange
            var command = new PerformLessonCommand(Guid.Empty, Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("O ID da matrícula é obrigatório.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be valid when both IDs are provided")]
        [Trait("Category", "PerformLessonCommand")]
        public void Command_Valid_ShouldBeValid()
        {
            // Arrange
            var command = new PerformLessonCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
        }
    }
}
