using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.FinancialManagement.Core;

namespace PlataformaEducacional.FinancialManagement.Core.Tests
{
    public class CardDataTests
    {
        private const string VALID_NAME = "Test Cardholder";
        private const string VALID_NUMBER = "1234567890123456";
        private const string VALID_EXPIRATION = "12/25";
        private const string VALID_CVV = "123";

        [Fact(DisplayName = "Constructor should initialize all properties when data is valid")]
        [Trait("Category", "Financial Management - CardData")]
        public void Constructor_ShouldInitializeProperties_WhenDataIsValid()
        {
            var cardData = new CardData(
                VALID_NAME,
                VALID_NUMBER,
                VALID_EXPIRATION,
                VALID_CVV
            );

            Assert.Equal(VALID_NAME, cardData.CardholderName);
            Assert.Equal(VALID_NUMBER, cardData.CardNumber);
            Assert.Equal(VALID_EXPIRATION, cardData.CardExpiration);
            Assert.Equal(VALID_CVV, cardData.CardCvv);
        }

        [Theory(DisplayName = "Should throw exception when cardholder name is invalid")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [Trait("Category", "Financial Management - CardData")]
        public void Constructor_ShouldThrowException_WhenCardholderNameIsInvalid(string invalidName)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new CardData(
                    invalidName,
                    VALID_NUMBER,
                    VALID_EXPIRATION,
                    VALID_CVV
                )
            );

            Assert.Equal("Cardholder name is required.", ex.Message);
        }

        [Theory(DisplayName = "Should throw exception when card number is invalid")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [Trait("Category", "Financial Management - CardData")]
        public void Constructor_ShouldThrowException_WhenCardNumberIsInvalid(string invalidNumber)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new CardData(
                    VALID_NAME,
                    invalidNumber,
                    VALID_EXPIRATION,
                    VALID_CVV
                )
            );

            Assert.Equal("Card number is required.", ex.Message);
        }

        [Theory(DisplayName = "Should throw exception when card expiration is invalid")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [Trait("Category", "Financial Management - CardData")]
        public void Constructor_ShouldThrowException_WhenCardExpirationIsInvalid(string invalidExpiration)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new CardData(
                    VALID_NAME,
                    VALID_NUMBER,
                    invalidExpiration,
                    VALID_CVV
                )
            );

            Assert.Equal("Card expiration is required.", ex.Message);
        }

        [Theory(DisplayName = "Should throw exception when card CVV is invalid")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [Trait("Category", "Financial Management - CardData")]
        public void Constructor_ShouldThrowException_WhenCardCvvIsInvalid(string invalidCvv)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new CardData(
                    VALID_NAME,
                    VALID_NUMBER,
                    VALID_EXPIRATION,
                    invalidCvv
                )
            );

            Assert.Equal("Card CVV is required.", ex.Message);
        }
    }
}
