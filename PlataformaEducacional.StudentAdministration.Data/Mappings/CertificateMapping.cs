using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacional.StudentAdministration.Domain;

namespace PlataformaEducacional.StudentAdministration.Data.Mappings
{
    public class CertificateMapping : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.ToTable("Certificates");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.VerificationCode)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.HasIndex(c => c.VerificationCode)
                .IsUnique();

            builder.HasOne(c => c.Enrollment)
                .WithOne(e => e.Certificate)
                .HasForeignKey<Certificate>(c => c.EnrollmentId)
                .IsRequired();
        }
    }
}
