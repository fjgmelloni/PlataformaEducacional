namespace PlataformaEducacional.Configurations
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(cors =>
            {
                var origins = configuration.GetSection("CorsOrigins")
                    .GetChildren()
                    .ToArray()
                    .Select(c => c.Value)
                    .ToArray();

                // Mensagem de iteração mantida em português
                Console.WriteLine("Iniciando CORS {0}", origins?.Aggregate((a, p) => $"{a}, {p}"));

                cors.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .WithHeaders("Origin", "X-Requested-With", "x-xsrf-token", "Content-Type", "Accept", "Authorization")
                        .WithOrigins(origins!)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .SetPreflightMaxAge(TimeSpan.FromDays(10))
                        .Build();
                });
            });

            return services;
        }
    }
}
