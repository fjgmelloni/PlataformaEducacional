using PlataformaEducacional.Core.Domain;

namespace PlataformaEducacional.FinancialManagement.Core
{
    public class Payment : Entity, IAggregateRoot
    {
        protected Payment() { }

        public Payment(Guid enrollmentId, decimal amount, CardData cardData)
        {
            EnrollmentId = enrollmentId;
            Amount = amount;
            CardData = cardData;

            Validate();
        }

        public Guid EnrollmentId { get; private set; }
        public decimal Amount { get; private set; }
        public CardData CardData { get; private set; } = null!;
        public Transaction Transaction { get; private set; } = null!;

        private void Validate()
        {
            Guard.AgainstEmpty(EnrollmentId, "EnrollmentId is required.");
            Guard.AgainstLessOrEqual(Amount, 0, "Amount must be greater than zero.");
            if (CardData is null)
                throw new DomainException("Card data is required.");
        }
    }
}
