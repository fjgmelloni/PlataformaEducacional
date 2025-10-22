using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Data.Mappings
{
    public class LessonProgressMapping : IEntityTypeConfiguration<LessonProgress>
    {
        public void Configure(EntityTypeBuilder<LessonProgress> builder)
        {
            builder.ToTable("LessonProgresses");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.LessonId)
                .IsRequired();

            builder.Property(p => p.CompletionDate)
                .IsRequired();

            builder.HasOne(p => p.Enrollment)
                .WithMany(e => e.LessonProgresses)
                .HasForeignKey(p => p.EnrollmentId);
        }
    }
}
