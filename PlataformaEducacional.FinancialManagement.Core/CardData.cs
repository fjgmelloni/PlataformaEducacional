using PlataformaEducacional.Core.Domain;

namespace PlataformaEducacional.FinancialManagement.Core
{
    public class CardData
    {
        public CardData(string cardholderName, string cardNumber, string cardExpiration, string cardCvv)
        {
            CardholderName = cardholderName;
            CardNumber = cardNumber;
            CardExpiration = cardExpiration;
            CardCvv = cardCvv;

            Validate();
        }

        public string CardholderName { get; private set; } = null!;
        public string CardNumber { get; private set; } = null!;
        public string CardExpiration { get; private set; } = null!;
        public string CardCvv { get; private set; } = null!;

        private void Validate()
        {
            Guard.AgainstNullOrWhiteSpace(CardholderName, "Cardholder name is required.");
            Guard.AgainstNullOrWhiteSpace(CardNumber, "Card number is required.");
            Guard.AgainstNullOrWhiteSpace(CardExpiration, "Card expiration is required.");
            Guard.AgainstNullOrWhiteSpace(CardCvv, "Card CVV is required.");
        }
    }
}
