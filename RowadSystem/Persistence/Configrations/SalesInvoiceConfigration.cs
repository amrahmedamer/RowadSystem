using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class SalesInvoiceConfigration : IEntityTypeConfiguration<SalesInvoice>
{
    public void Configure(EntityTypeBuilder<SalesInvoice> builder)
    {
        builder
            .Property(i => i.DiscountAmount)
            .HasPrecision(18, 4);

        builder
             .Property(i => i.NetAmount)
            .HasPrecision(18, 4);

        builder
            .Property(i => i.TotalAmount)
            .HasPrecision(18, 4);

    }
}
