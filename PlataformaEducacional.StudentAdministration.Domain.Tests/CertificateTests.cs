using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Domain.Tests
{
    public class CertificateTests
    {
        private readonly Guid _validEnrollmentId = Guid.NewGuid();

        [Fact(DisplayName = "Constructor should assign correct EnrollmentId")]
        [Trait("Category", "Certificate - Constructor")]
        public void Certificate_NewCertificate_ShouldAssignEnrollmentId()
        {
            // Arrange & Act
            var certificate = new Certificate(_validEnrollmentId);

            // Assert
            Assert.NotNull(certificate);
            Assert.Equal(_validEnrollmentId, certificate.EnrollmentId);
        }

        [Fact(DisplayName = "Constructor should generate a non-empty VerificationCode")]
        [Trait("Category", "Certificate - Constructor")]
        public void Certificate_NewCertificate_ShouldGenerateVerificationCode()
        {
            // Arrange & Act
            var certificate = new Certificate(_validEnrollmentId);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(certificate.VerificationCode));
        }

        [Fact(DisplayName = "Constructor should generate unique verification codes")]
        [Trait("Category", "Certificate - Constructor")]
        public void Certificate_NewCertificate_ShouldGenerateDifferentVerificationCodes()
        {
            // Arrange & Act
            var certificate1 = new Certificate(_validEnrollmentId);
            var certificate2 = new Certificate(_validEnrollmentId);

            // Assert
            Assert.NotEqual(certificate1.VerificationCode, certificate2.VerificationCode);
        }

        [Fact(DisplayName = "Constructor should throw DomainException when EnrollmentId is invalid")]
        [Trait("Category", "Certificate - Constructor")]
        public void Certificate_NewCertificate_ShouldThrowWhenEnrollmentIdInvalid()
        {
            // Arrange & Act
            var ex = Assert.Throws<DomainException>(() => new Certificate(Guid.Empty));

            // Assert
            Assert.Equal("The enrollment ID is required.", ex.Message);
        }
    }
}
