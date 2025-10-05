using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class OrderItemConfigration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        //builder
        //    .Property(o => o.Total)
        //    .HasComputedColumnSql("[Quantity] * [Price]");

        builder
         .Property(x => x.Price)
         .HasPrecision(18, 2);

        builder
        .Property(o => o.Total)
        .HasPrecision(18, 4);

    }
}
