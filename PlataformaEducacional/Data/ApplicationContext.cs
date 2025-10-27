using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PlataformaEducacional.Api.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options)
        : IdentityDbContext(options)
    { }
}
