using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class ShoppingCartItemConfigration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        //builder
        //.HasOne(i => i.ShoppingCart)
        //.WithMany(c => c.shoppingCartItems)
        //.HasForeignKey(i => i.ShoppingCartId)
        //.OnDelete(DeleteBehavior.Cascade);

        builder
        .Property(s => s.Total)
        .HasPrecision(18, 4);

        builder
        .Property(s => s.Price)
        .HasPrecision(18, 4);



    }
}
