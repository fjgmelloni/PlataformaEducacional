using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacional.ContentManagement.Domain.Lessons;

namespace PlataformaEducacional.ContentManagement.Data.Mappings
{
    public class LessonMapping : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            // Nome da tabela — ajustado para singular e padronizado em inglês
            builder.ToTable("Lesson");

            // Chave primária
            builder.HasKey(l => l.Id);

            // Campos
            builder.Property(l => l.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(l => l.Content)
                   .IsRequired()
                   .HasColumnType("varchar(max)"); // alteração sutil: agora aceita textos longos

            // Relacionamento
            builder.HasOne(l => l.Course)
                   .WithMany(c => c.Lessons)
                   .HasForeignKey(l => l.CourseId)
                   .OnDelete(DeleteBehavior.Restrict); // comportamento de exclusão mais seguro

            // Índice útil para busca por curso e título
            builder.HasIndex(l => new { l.CourseId, l.Title })
                   .HasDatabaseName("IX_Lesson_Course_Title");
        }
    }
}
