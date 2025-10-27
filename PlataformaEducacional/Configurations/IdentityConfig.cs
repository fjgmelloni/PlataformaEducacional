using Microsoft.AspNetCore.Identity;
using PlataformaEducacional.Api.Data;

namespace PlataformaEducacional.Api.Configurations
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            return services;
        }
    }
}
