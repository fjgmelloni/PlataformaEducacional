using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.CompleteEnrollment;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.CompleteEnrollment
{
    public class CompleteEnrollmentCommandTests
    {
        [Fact(DisplayName = "Must be invalid when StudentId is Guid.Empty")]
        [Trait("Category", "CompleteEnrollmentCommand")]
        public void StudentId_WhenEmpty_ShouldBeInvalid()
        {
            // Arrange
            var command = new CompleteEnrollmentCommand(Guid.NewGuid(), Guid.Empty);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("Student is required.", command.ValidationResult.Errors);

        }

        [Fact(DisplayName = "Must be valid when StudentId is valid")]
        [Trait("Category", "CompleteEnrollmentCommand")]
        public void StudentId_WhenValid_ShouldBeValid()
        {
            // Arrange
            var command = new CompleteEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Must be invalid when EnrollmentId is Guid.Empty")]
        [Trait("Category", "CompleteEnrollmentCommand")]
        public void EnrollmentId_WhenEmpty_ShouldBeInvalid()
        {
            // Arrange
            var command = new CompleteEnrollmentCommand(Guid.Empty, Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("Enrollment is required.", command.ValidationResult.Errors);

        }
    }
}
