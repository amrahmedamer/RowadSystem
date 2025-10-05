using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class CouponConfigration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder
         .Property(c => c.DiscountValue)
         .HasPrecision(18, 4);


    }
}
