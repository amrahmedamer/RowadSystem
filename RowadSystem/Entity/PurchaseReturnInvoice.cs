namespace RowadSystem.Entity;

public class PurchaseReturnInvoice : Invoice
{
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public int PurchaseInvoiceId { get; set; }
    public PurchaseInvoice? PurchaseInvoice { get; set; }
    public ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; } = [];
}
