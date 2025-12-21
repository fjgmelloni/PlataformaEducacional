using Moq;
using PlataformaEducacional.FinancialManagement.Core;
using PlataformaEducacional.FinancialManagement.Integration;

namespace PlataformaEducacional.FinancialManagement.Integration.Tests
{
    public class CreditCardPaymentFacadeTests
    {
        private readonly Mock<IPayPalGateway> _payPalGatewayMock;
        private readonly Mock<IConfigurationManager> _configManagerMock;
        private readonly CreditCardPaymentFacade _facade;

        public CreditCardPaymentFacadeTests()
        {
            _payPalGatewayMock = new Mock<IPayPalGateway>();
            _configManagerMock = new Mock<IConfigurationManager>();

            _configManagerMock.Setup(c => c.GetValue("apiKey")).Returns("api-key");
            _configManagerMock.Setup(c => c.GetValue("encryptionKey")).Returns("encryption-key");

            _payPalGatewayMock
                .Setup(g => g.GetPayPalServiceKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("service-key");

            _payPalGatewayMock
                .Setup(g => g.GetCardHashKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("card-hash");

            _facade = new CreditCardPaymentFacade(
                _payPalGatewayMock.Object,
                _configManagerMock.Object
            );
        }

        [Fact(DisplayName = "Should return paid transaction when gateway commit succeeds")]
        [Trait("Category", "Financial Management - CreditCardPaymentFacade")]
        public void Charge_ShouldReturnPaidTransaction_WhenCommitSucceeds()
        {
            // Arrange
            var payment = new Payment(
                Guid.NewGuid(),
                500,
                new CardData("Felicio", "1234567890", "12/29", "123")
            );

            _payPalGatewayMock
                .Setup(g => g.CommitTransaction(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>()
                ))
                .Returns(true);

            // Act
            var transaction = _facade.Charge(Guid.NewGuid(), payment);

            // Assert
            Assert.Equal(TransactionStatus.Paid, transaction.Status);
        }

        [Fact(DisplayName = "Should return declined transaction when gateway commit fails")]
        [Trait("Category", "Financial Management - CreditCardPaymentFacade")]
        public void Charge_ShouldReturnDeclinedTransaction_WhenCommitFails()
        {
            // Arrange
            var payment = new Payment(
                Guid.NewGuid(),
                500,
                new CardData("Felicio", "1234567890", "12/29", "123")
            );

            _payPalGatewayMock
                .Setup(g => g.CommitTransaction(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>()
                ))
                .Returns(false);

            // Act
            var transaction = _facade.Charge(Guid.NewGuid(), payment);

            // Assert
            Assert.Equal(TransactionStatus.Declined, transaction.Status);
        }
    }
}
