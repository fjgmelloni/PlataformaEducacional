using PlataformaEducacional.Core.Domain;

namespace PlataformaEducacional.Core.Tests
{
    public class DomainExceptionTests
    {
        [Fact(DisplayName = nameof(DomainException_Constructor_ShouldCreateExceptionWithoutMessage))]
        [Trait("Category", "Content Management - Core - DomainException")]
        public void DomainException_Constructor_ShouldCreateExceptionWithoutMessage()
        {
            // Arrange & Act
            var exception = new DomainException();

            // Assert
            Assert.NotNull(exception.Message);
        }

        [Fact(DisplayName = nameof(DomainException_Constructor_ShouldCreateExceptionWithMessage))]
        [Trait("Category", "Content Management - Core - DomainException")]
        public void DomainException_Constructor_ShouldCreateExceptionWithMessage()
        {
            // Arrange
            var errorMessage = "Domain exception message.";

            // Act
            var exception = new DomainException(errorMessage);

            // Assert
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact(DisplayName = nameof(DomainException_Constructor_ShouldCreateExceptionWithMessageAndInnerException))]
        [Trait("Category", "Content Management - Core - DomainException")]
        public void DomainException_Constructor_ShouldCreateExceptionWithMessageAndInnerException()
        {
            // Arrange
            var errorMessage = "Message with inner exception.";
            var innerException = new Exception("Inner exception message.");

            // Act
            var exception = new DomainException(errorMessage, innerException);

            // Assert
            Assert.Equal(errorMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}
