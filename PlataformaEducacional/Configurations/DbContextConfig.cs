using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Api.Data;
using PlataformaEducacional.StudentAdministration.Data;
using PlataformaEducacional.FinancialManagement.Data;
using PlataformaEducacional.ContentManagement.Data.Context;

namespace PlataformaEducacional.Configurations
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ContentContext>(opt =>
            {
                opt.UseSqlite(connectionString);
                opt.EnableSensitiveDataLogging();
            });

            services.AddDbContext<StudentAdministrationContext>(opt =>
            {
                opt.UseSqlite(connectionString);
                opt.EnableSensitiveDataLogging();
            });

            services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlite(connectionString);
                opt.EnableSensitiveDataLogging();
            });

            services.AddDbContext<PaymentContext>(opt =>
            {
                opt.UseSqlite(connectionString);
                opt.EnableSensitiveDataLogging();
            });

            return services;
        }

        public static IApplicationBuilder UseIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
