using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class ProductUnitConfigration : IEntityTypeConfiguration<ProductUnit>
{
    public void Configure(EntityTypeBuilder<ProductUnit> builder)
    {
        builder.HasKey(pu => new { pu.ProductId, pu.UnitId });

        builder
         .Property(x => x.SellingPrice)
         .HasPrecision(18, 2);

    }
}
