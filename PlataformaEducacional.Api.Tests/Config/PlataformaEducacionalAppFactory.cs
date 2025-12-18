using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlataformaEducacional.Api.Configurations;
using PlataformaEducacional.Api.Data;
using PlataformaEducacional.StudentAdministration.Data;
using PlataformaEducacional.ContentManagement.Data;
using PlataformaEducacional.FinancialManagement.Data;
using PlataformaEducacional.ContentManagement.Data;
using PlataformaEducacional.ContentManagement.Data.Context;
using PlataformaEducacional.Configurations;

namespace PlataformaEducacional.Api.Tests.Config
{
    public class PlataformaEducacionalAppFactory<TProgram>
        : WebApplicationFactory<TProgram>, IDisposable
        where TProgram : class
    {
        private SqliteConnection _connection = null!;

        public PlataformaEducacionalAppFactory()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextTypes = new[]
                {
                    typeof(ApplicationContext),
                    typeof(ContentContext),
                    typeof(StudentAdministrationContext),
                    typeof(PaymentContext)
                };

                foreach (var dbContextType in dbContextTypes)
                {
                    var descriptorsToRemove = services
                        .Where(d =>
                            d.ServiceType == dbContextType ||
                            (d.ServiceType.IsGenericType &&
                             d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>) &&
                             d.ServiceType.GenericTypeArguments[0] == dbContextType) ||
                            d.ServiceType == typeof(DbContextOptions))
                        .ToList();

                    foreach (var descriptor in descriptorsToRemove)
                    {
                        services.Remove(descriptor);
                    }
                }

                services.AddDbContext<ApplicationContext>(options =>
                    options.UseSqlite(_connection));

                services.AddDbContext<ContentContext>(options =>
                    options.UseSqlite(_connection));

                services.AddDbContext<StudentAdministrationContext>(options =>
                    options.UseSqlite(_connection));

                services.AddDbContext<PaymentContext>(options =>
                    options.UseSqlite(_connection));

                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    DbMigrationHelper.EnsureSeedData(serviceProvider)
                        .GetAwaiter()
                        .GetResult();
                }
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            return base.CreateHost(builder);
        }

        public new void Dispose()
        {
            base.Dispose();

            if (_connection != null)
            {
                _connection.Close();
            }
        }
    }
}
