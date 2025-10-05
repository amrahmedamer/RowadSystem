namespace RowadSystem.Entity;

public class PurchaseInvoiceDetail : InvoiceDetail
{
    public int PurchaseInvoiceId { get; set; }
    public PurchaseInvoice? PurchaseInvoice { get; set; }


}
