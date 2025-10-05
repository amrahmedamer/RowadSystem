namespace RowadSystem.Entity;

public class PurchaseReturnDetail : InvoiceDetail
{
    public int PurchaseReturnId { get; set; }
    public PurchaseReturnInvoice? PurchaseReturn { get; set; }

}
