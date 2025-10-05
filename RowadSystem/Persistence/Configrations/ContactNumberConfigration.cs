using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class ContactNumberConfigration : IEntityTypeConfiguration<ContactNumber>
{
    public void Configure(EntityTypeBuilder<ContactNumber> builder)
    {
        builder
        .HasOne(c => c.User)
        .WithMany(u => u.ContactNumbers)
        .HasForeignKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(c => c.Customer)
            .WithMany(cu => cu.ContactNumbers)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(c => c.Supplier)
            .WithMany(s => s.ContactNumbers)
            .HasForeignKey(c => c.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}
