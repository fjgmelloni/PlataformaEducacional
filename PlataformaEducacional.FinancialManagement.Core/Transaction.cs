using PlataformaEducacional.Core.Domain;

namespace PlataformaEducacional.FinancialManagement.Core
{
    public class Transaction : Entity
    {
        protected Transaction() { }

        public Transaction(Guid paymentId, decimal total)
        {
            PaymentId = paymentId;
            Total = total;

            Validate();
        }

        public Guid PaymentId { get; private set; }
        public decimal Total { get; private set; }
        public TransactionStatus Status { get; private set; } = TransactionStatus.Paid;
        public Payment Payment { get; private set; } = null!;

        public void ChangeStatus(TransactionStatus status)
        {
            Status = status;
        }

        private void Validate()
        {
            Guard.AgainstEmpty(PaymentId, "PaymentId is required.");
            Guard.AgainstLessOrEqual(Total, 0, "Total must be greater than zero.");
        }
    }
}
