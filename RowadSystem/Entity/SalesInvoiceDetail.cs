namespace RowadSystem.Entity;

public class SalesInvoiceDetail : InvoiceDetail
{
    public int SalesInvoiceId { get; set; }
    public SalesInvoice? SalesInvoice { get; set; }

}
