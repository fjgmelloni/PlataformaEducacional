using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.FinancialManagement.Core;

namespace PlataformaEducacional.FinancialManagement.Core.Tests
{
    public class PaymentTests
    {
        private readonly Guid _validEnrollmentId = Guid.NewGuid();
        private const decimal VALID_AMOUNT = 100.50m;

        private readonly CardData _validCardData =
            new CardData(
                "Test User",
                "1234123412341234",
                "12/26",
                "123"
            );

        [Fact(DisplayName = "Constructor should create payment when data is valid")]
        [Trait("Category", "Financial Management - Payment")]
        public void Constructor_ShouldCreatePayment_WhenDataIsValid()
        {
            // Act
            var payment = new Payment(
                _validEnrollmentId,
                VALID_AMOUNT,
                _validCardData
            );

            // Assert
            Assert.NotNull(payment);
            Assert.Equal(_validEnrollmentId, payment.EnrollmentId);
            Assert.Equal(VALID_AMOUNT, payment.Amount);
            Assert.Equal(_validCardData, payment.CardData);
        }

        [Theory(DisplayName = "Constructor should throw exception when amount is invalid")]
        [InlineData(0)]
        [InlineData(-0.01)]
        [InlineData(-100)]
        [Trait("Category", "Financial Management - Payment")]
        public void Constructor_ShouldThrowException_WhenAmountIsLessOrEqualZero(decimal invalidAmount)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Payment(
                    _validEnrollmentId,
                    invalidAmount,
                    _validCardData
                )
            );

            Assert.Equal("Amount must be greater than zero.", ex.Message);
        }

        [Fact(DisplayName = "Constructor should throw exception when enrollment id is empty")]
        [Trait("Category", "Financial Management - Payment")]
        public void Constructor_ShouldThrowException_WhenEnrollmentIdIsEmpty()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Payment(
                    Guid.Empty,
                    VALID_AMOUNT,
                    _validCardData
                )
            );

            Assert.Equal("EnrollmentId is required.", ex.Message);
        }

        [Fact(DisplayName = "Constructor should throw exception when card data is null")]
        [Trait("Category", "Financial Management - Payment")]
        public void Constructor_ShouldThrowException_WhenCardDataIsNull()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Payment(
                    _validEnrollmentId,
                    VALID_AMOUNT,
                    null!
                )
            );

            Assert.Equal("Card data is required.", ex.Message);
        }
    }
}
