namespace PlataformaEducacional.FinancialManagement.Integration
{
    public interface IPayPalGateway
    {
        string GetPayPalServiceKey(string apiKey, string encryptionKey);
        string GetCardHashKey(string serviceKey, string creditCardNumber);
        bool CommitTransaction(string cardHashKey, string orderId, decimal amount);
    }
}
