namespace RowadSystem.Entity;

public class SalesReturnInvoiceDetail : InvoiceDetail
{
    public int SalesReturnId { get; set; }
    public SalesReturnInvoice? SalesReturn { get; set; }

}
