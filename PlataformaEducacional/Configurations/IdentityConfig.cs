using Microsoft.AspNetCore.Identity;
using PlataformaEducacao.Api.Data;

namespace PlataformaEducacional.Api.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();
            return services;
        }
    }
}
