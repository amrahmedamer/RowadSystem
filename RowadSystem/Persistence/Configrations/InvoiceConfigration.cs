using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class InvoiceConfigration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder
            .HasIndex(x=>x.InvoiceNumber) ;
        builder
           .Property(i => i.TotalAmount)
           .HasPrecision(18, 4);

    }
}
