using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CancelEnrollment;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.CancelEnrollment
{
    public class CancelEnrollmentCommandTests
    {
        [Fact(DisplayName = "Valid command should return IsValid = True")]
        [Trait("Category", "CancelEnrollmentCommand")]
        public void Command_ValidFields_ShouldBeValid()
        {
            // Arrange
            var command = new CancelEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
            Assert.True(command.ValidationResult.IsValid);
        }

        [Fact(DisplayName = "Invalid command should return IsValid = False")]
        [Trait("Category", "CancelEnrollmentCommand")]
        public void Command_InvalidFields_ShouldBeInvalid()
        {
            // Arrange
            var command = new CancelEnrollmentCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.False(command.ValidationResult.IsValid);
            Assert.Equal(2, command.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "Should be invalid when EnrollmentId is empty")]
        [Trait("Category", "CancelEnrollmentCommand")]
        public void Command_InvalidEnrollment_ShouldReturnError()
        {
            // Arrange
            var command = new CancelEnrollmentCommand(Guid.Empty, Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("Enrollment ID is required.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be invalid when StudentId is empty")]
        [Trait("Category", "CancelEnrollmentCommand")]
        public void Command_InvalidStudent_ShouldReturnError()
        {
            // Arrange
            var command = new CancelEnrollmentCommand(Guid.NewGuid(), Guid.Empty);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("Student ID is required.", command.ValidationResult.Errors);

        }
    }
}
