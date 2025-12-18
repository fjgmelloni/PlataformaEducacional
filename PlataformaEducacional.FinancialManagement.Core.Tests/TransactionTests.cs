using PlataformaEducacional.Core.Domain;
using PlataformaEducacional.FinancialManagement.Core;

namespace PlataformaEducacional.FinancialManagement.Core.Tests
{
    public class TransactionTests
    {
        private readonly Guid _validPaymentId = Guid.NewGuid();
        private const decimal VALID_TOTAL = 50.00m;

        [Fact(DisplayName = "Constructor should create transaction with default status")]
        [Trait("Category", "Financial Management - Transaction")]
        public void Constructor_ShouldCreateTransaction_WhenArgumentsAreValid()
        {
            // Act
            var transaction = new Transaction(
                _validPaymentId,
                VALID_TOTAL
            );

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(_validPaymentId, transaction.PaymentId);
            Assert.Equal(VALID_TOTAL, transaction.Total);
            Assert.Equal(TransactionStatus.Paid, transaction.Status);
        }

        [Fact(DisplayName = "ChangeStatus should update transaction status")]
        [Trait("Category", "Financial Management - Transaction")]
        public void ChangeStatus_ShouldUpdateStatusCorrectly()
        {
            // Arrange
            var transaction = new Transaction(_validPaymentId, VALID_TOTAL);

            // Act
            transaction.ChangeStatus(TransactionStatus.Declined);

            // Assert
            Assert.Equal(TransactionStatus.Declined, transaction.Status);
        }

        [Theory(DisplayName = "Constructor should throw exception when total is invalid")]
        [InlineData(0)]
        [InlineData(-1)]
        [Trait("Category", "Financial Management - Transaction")]
        public void Constructor_ShouldThrowException_WhenTotalIsLessOrEqualZero(decimal invalidTotal)
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Transaction(
                    _validPaymentId,
                    invalidTotal
                )
            );

            Assert.Equal("Total must be greater than zero.", ex.Message);
        }

        [Fact(DisplayName = "Constructor should throw exception when payment id is empty")]
        [Trait("Category", "Financial Management - Transaction")]
        public void Constructor_ShouldThrowException_WhenPaymentIdIsEmpty()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Transaction(
                    Guid.Empty,
                    VALID_TOTAL
                )
            );

            Assert.Equal("PaymentId is required.", ex.Message);
        }
    }
}
