using System;

namespace PlataformaEducacional.Core.Domain
{
    public static class Guard
    {
        public static void AgainstNullOrWhiteSpace(string? value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException(message);
        }

        public static void AgainstLessOrEqual(decimal value, decimal minimum, string message)
        {
            if (value <= minimum)
                throw new DomainException(message);
        }

        public static void AgainstLessOrEqual(int value, int minimum, string message)
        {
            if (value <= minimum)
                throw new DomainException(message);
        }

        public static void AgainstEmpty(Guid? id, string message)
        {
            if (id is null || id == Guid.Empty)
                throw new DomainException(message);
        }
    }
}
