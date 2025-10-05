using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RowadSystem.Persistence.Configrations;

public class PurchaseInvoiceDetailConfigration : IEntityTypeConfiguration<PurchaseInvoiceDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseInvoiceDetail> builder)
    {
        //builder
        //    .HasOne(d => d.PurchaseInvoice)
        //    .WithMany(r => r.Details)
        //    .HasForeignKey(d => d.PurchaseInvoiceId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //builder
        //    .HasOne(d => d.Product)
        //    .WithMany()
        //    .HasForeignKey(d => d.ProductId)
        //    .OnDelete(DeleteBehavior.Restrict); 

    }
}
