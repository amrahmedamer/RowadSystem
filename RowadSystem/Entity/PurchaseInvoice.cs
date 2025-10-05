namespace RowadSystem.Entity;

public class PurchaseInvoice : Invoice
{
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public ICollection<PurchaseInvoiceDetail> purchaseInvoiceDetails { get; set; } = [];
    public ICollection<PurchaseReturnInvoice> PurchaseReturns { get; set; } = [];
}
