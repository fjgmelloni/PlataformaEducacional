using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.AddStudent;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.AddStudent
{
    public class AddStudentCommandTests
    {
        [Fact(DisplayName = "AddStudent Command Valid")]
        [Trait("Category", "AddStudentCommand")]
        public void AddStudentCommand_IsValid_WhenCorrectData()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.NewGuid(), "Rinaldo");

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Should be invalid when name is empty")]
        [Trait("Category", "AddStudentCommand")]
        public void AddStudentCommand_ShouldBeInvalid_WhenNameEmpty()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.NewGuid(), "");

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("Student name is required.", command.ValidationResult.Errors);

        }

        [Fact(DisplayName = "Should be invalid when UserId is empty")]
        [Trait("Category", "AddStudentCommand")]
        public void AddStudentCommand_ShouldBeInvalid_WhenUserIdEmpty()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.Empty, "Rinaldo");

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("Student name is required.", command.ValidationResult.Errors);
        }
    }
}
