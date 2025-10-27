﻿namespace PlataformaEducacional.Api.Extensions
{
    public class JwtSettings
    {
        public string Secret { get; set; } = null!;

        public int ExpirationHours { get; set; }

        public string Issuer { get; set; } = null!;

        public string Audience { get; set; } = null!;
    }
}
