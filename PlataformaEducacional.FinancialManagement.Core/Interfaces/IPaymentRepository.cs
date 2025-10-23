using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.FinancialManagement.Core
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void Add(Payment payment);
        void AddTransaction(Transaction transaction);
    }
}
