using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Data;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.StudentAdministration.Domain;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PlataformaEducacional.StudentAdministration.Data
{
    public class StudentAdministrationContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public StudentAdministrationContext(DbContextOptions<StudentAdministrationContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<LessonProgress> LessonProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // All string columns use varchar(255)
            foreach (var property in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(255)");
            }

            // Apply all mapping configurations in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentAdministrationContext).Assembly);

            // Ignore Event class from Core
            modelBuilder.Ignore<Event>();

            // Set DeleteBehavior = ClientCascade for all foreign keys
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientCascade;
            }

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;
            if (success)
                await _mediatorHandler.PublishDomainEvents(this);

            return success;
        }
    }
}
