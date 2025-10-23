namespace PlataformaEducacional.FinancialManagement.Core
{
    public interface ICreditCardPaymentFacade
    {
        Transaction Charge(Guid enrollmentId, Payment payment);
    }
}
