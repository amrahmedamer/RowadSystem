using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class DiscountConfigration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {

        builder
            .Property(d => d.Value)
            .HasPrecision(18, 4);


    }
}
