using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteCourse;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.CompleteCourse
{
    public class CompleteCourseCommandTests
    {
        [Fact(DisplayName = "Command should be invalid when EnrollmentId is empty")]
        public void Command_InvalidEnrollmentId_ShouldBeInvalid()
        {
            // Arrange
            var command = new CompleteCourseCommand(Guid.Empty, Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("O ID da matrícula é obrigatório.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Command should be invalid when StudentId is empty")]
        public void Command_InvalidStudentId_ShouldBeInvalid()
        {
            // Arrange
            var command = new CompleteCourseCommand(Guid.NewGuid(), Guid.Empty);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("O ID do aluno é obrigatório.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Command should be valid when data is correct")]
        public void Command_Valid_ShouldBeValid()
        {
            // Arrange
            var command = new CompleteCourseCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
        }
    }
}
