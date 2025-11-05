using PlataformaEducacional.StudentAdministration.Application.Features.Students.Commands.GenerateCertificate;

namespace PlataformaEducacional.StudentAdministration.Application.Tests.Commands.GenerateCertificate
{
    public class GenerateCertificateCommandTests
    {
        [Fact(DisplayName = "Should be invalid when EnrollmentId is empty")]
        public void Should_Be_Invalid_When_EnrollmentId_Empty()
        {
            // Arrange
            var command = new GenerateCertificateCommand(Guid.Empty);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains("O ID da matrícula é obrigatório.", command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be valid when EnrollmentId is provided")]
        public void Should_Be_Valid()
        {
            var command = new GenerateCertificateCommand(Guid.NewGuid());
            Assert.True(command.IsValid());
        }
    }
}
