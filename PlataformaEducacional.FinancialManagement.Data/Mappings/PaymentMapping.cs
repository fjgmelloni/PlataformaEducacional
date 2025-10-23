using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacional.FinancialManagement.Core;

namespace PlataformaEducacional.FinancialManagement.Data.Mappings
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(p => p.CardData, cc =>
            {
                cc.Property(c => c.CardNumber)
                  .HasColumnName("CardNumber")
                  .HasColumnType("varchar(16)")
                  .IsRequired();

                cc.Property(c => c.CardholderName)
                  .HasColumnName("CardholderName")
                  .HasColumnType("varchar(250)")
                  .IsRequired();

                cc.Property(c => c.CardExpiration)
                  .HasColumnName("CardExpiration")
                  .HasColumnType("varchar(10)")
                  .IsRequired();

                cc.Property(c => c.CardCvv)
                  .HasColumnName("CardCvv")
                  .HasColumnType("varchar(4)")
                  .IsRequired();
            });

            // 1:1 => Payment : Transaction
            builder.HasOne(c => c.Transaction)
                   .WithOne(c => c.Payment);

            builder.ToTable("Payments");
        }
    }
}
