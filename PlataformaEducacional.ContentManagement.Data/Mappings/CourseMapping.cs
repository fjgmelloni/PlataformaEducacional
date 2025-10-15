using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacional.ContentManagement.Domain.Courses;

namespace PlataformaEducacional.ContentManagement.Data.Mappings
{
    public class CourseMapping : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Price)
                .IsRequired();

            builder.Property(p => p.IsAvailable)
                .IsRequired();

            builder.OwnsOne(c => c.Syllabus, syllabus =>
            {
                syllabus.Property(s => s.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(1000);

                syllabus.Property(s => s.Workload)
                    .HasColumnName("Workload");
            });
        }
    }
}
