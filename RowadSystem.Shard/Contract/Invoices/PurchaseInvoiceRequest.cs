using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.Shard.Contract.Invoices;

public class PurchaseInvoiceRequest
{
    public int SalesInvoiceId { get; set; }
    public int SupplierId { get; set; }
    public string? Notes { get; set; }
    public List<PaymentRequest> Payments { get; set; }
    public List<PurchaseInvoiceItemRequest> Items { get; set; }

};
