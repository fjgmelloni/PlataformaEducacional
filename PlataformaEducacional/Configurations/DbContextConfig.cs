using Microsoft.EntityFrameworkCore;
using PlataformaEducacao.Api.Data;
using PlataformaEducacao.GestaoAluno.Data;
using PlataformaEducacao.GestaoConteudo.Data;
using PlataformaEducacao.GestaoFinanceira.Data;

namespace PlataformaEducacional.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GestaoConteudoContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                opt.EnableSensitiveDataLogging();
            });
            services.AddDbContext<GestaoAlunoContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                opt.EnableSensitiveDataLogging();
            });
            services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                opt.EnableSensitiveDataLogging();
            });
            services.AddDbContext<PagamentoContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
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
