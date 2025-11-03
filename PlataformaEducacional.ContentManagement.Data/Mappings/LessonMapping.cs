using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacional.ContentManagement.Domain.Lessons;

namespace PlataformaEducacional.ContentManagement.Data.Mappings
{
    public class LessonMapping : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(l => l.Content)
                   .IsRequired()
                   .HasColumnType("TEXT");

            builder.Property(l => l.Order)
                   .HasColumnName("SortOrder")
                   .IsRequired();

            builder.Property(l => l.Material)
                   .HasMaxLength(255)
                   .HasColumnType("varchar(255)");

            builder.HasOne(l => l.Course)
                   .WithMany(c => c.Lessons)
                   .HasForeignKey(l => l.CourseId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasIndex(l => new { l.CourseId, l.Title })
                   .HasDatabaseName("IX_Lesson_Course_Title");
        }
    }
}
