using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class InvoiceDetailConfigration : IEntityTypeConfiguration<InvoiceDetail>
{
    public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
    {
        builder
          .Property(x => x.Price)
          .HasPrecision(18, 2);

        builder
        .Property(i => i.Total)
        .HasPrecision(18, 4);

    }
}
