using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class ProductConfigration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
       .Property(x => x.PurchasePrice)
       .HasPrecision(18, 2);

        builder
           .HasIndex(x => x.Barcode);

    }
}
