using RowadSystem.Shard.Contract.Payments;


namespace RowadSystem.Shard.Contract.Invoices;
public class PurchaseInvoiceResponse
{

    public string InvoiceNumber { get; set; } = null!;
    public int PurchaseInvoiceId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public List<InvoiceItemResponse> Items { get; set; } = new List<InvoiceItemResponse>();
    public List<PaymentResponse> Payments { get; set; } = new();
}
