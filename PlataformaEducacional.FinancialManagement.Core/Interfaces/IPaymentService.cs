using PlataformaEducacional.Core.Domain.DTO;

namespace PlataformaEducacional.FinancialManagement.Core
{
    public interface IPaymentService
    {
        Task<Transaction> ProcessEnrollmentPayment(EnrollmentPayment payment);
    }
}
