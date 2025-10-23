namespace PlataformaEducacional.FinancialManagement.Integration
{
    public class ConfigurationManager : IConfigurationManager
    {
        public string GetValue(string node)
        {
            // Generates a random alphanumeric string of length 10
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable
                .Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
