using PlataformaEducacional.FinancialManagement.Core;

namespace PlataformaEducacional.FinancialManagement.Integration
{
    public class CreditCardPaymentFacade : ICreditCardPaymentFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configManager;

        public CreditCardPaymentFacade(IPayPalGateway payPalGateway, IConfigurationManager configManager)
        {
            _payPalGateway = payPalGateway;
            _configManager = configManager;
        }

        public Transaction Charge(Guid enrollmentId, Payment payment)
        {
            var apiKey = _configManager.GetValue("apiKey");
            var encryptionKey = _configManager.GetValue("encryptionKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encryptionKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, payment.CardData.CardNumber);

            var paymentResult = _payPalGateway.CommitTransaction(cardHashKey, payment.Id.ToString(), payment.Amount);

            // Simulates the payment transaction creation (the gateway should return this in a real scenario)
            var transaction = new Transaction(payment.Id, payment.Amount);

            if (paymentResult)
            {
                transaction.ChangeStatus(TransactionStatus.Paid);
                return transaction;
            }

            transaction.ChangeStatus(TransactionStatus.Declined);
            return transaction;
        }
    }
}
