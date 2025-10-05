using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class ShoppingCartConfigration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder
        .HasMany(x => x.shoppingCartItems)
        .WithOne(x => x.ShoppingCart)
        .HasForeignKey(x => x.ShoppingCartId);

        builder
        .Property(s => s.TotalAmount)
        .HasPrecision(18, 4);

    }
}
