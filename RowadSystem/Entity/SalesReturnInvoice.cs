namespace RowadSystem.Entity;

public class SalesReturnInvoice : Invoice
{
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int? SalesInvoiceId { get; set; }
    public SalesInvoice? SalesInvoice { get; set; }
    public ICollection<SalesReturnInvoiceDetail> salesReturnInvoiceDetails { get; set; }
}
