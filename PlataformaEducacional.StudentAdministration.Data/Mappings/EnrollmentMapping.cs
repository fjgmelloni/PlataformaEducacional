using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Data.Mappings
{
    public class EnrollmentMapping : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("Enrollments");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.CourseName)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(e => e.EnrollmentDate)
                .IsRequired();

            builder.Property(e => e.EnrollmentStatus)
                .IsRequired();

            builder.Property(e => e.CourseId)
                .IsRequired();

            builder.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            builder.OwnsOne(e => e.LearningHistory, lh =>
            {
                lh.Property(p => p.TotalLessons)
                    .IsRequired()
                    .HasDefaultValue(0)
                    .HasColumnName("TotalLessons");

                lh.Property(p => p.OverallProgress)
                    .IsRequired()
                    .HasDefaultValue(0)
                    .HasColumnName("OverallProgress");

                lh.Property(p => p.CourseStatus)
                    .IsRequired()
                    .HasDefaultValue(CourseStatus.NotStarted)
                    .HasColumnName("CourseStatus");

                lh.Property(p => p.CompletionDate)
                    .HasColumnName("CompletionDate");
            });
        }
    }
}
