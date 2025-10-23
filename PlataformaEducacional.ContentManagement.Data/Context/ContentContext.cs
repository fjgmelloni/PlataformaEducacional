using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.ContentManagement.Domain.Courses;
using PlataformaEducacional.ContentManagement.Domain.Lessons;
using PlataformaEducacional.Core.Data;
using PlataformaEducacional.Core.Messages.Base;

namespace PlataformaEducacao.ContentManagement.Data.Context
{
    public class ContentContext : DbContext, IUnitOfWork
    {
        public ContentContext(DbContextOptions<ContentContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(255)");
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContentContext).Assembly);
            modelBuilder.Ignore<Event>();

            foreach (var relationship in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientCascade;
            }

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}
