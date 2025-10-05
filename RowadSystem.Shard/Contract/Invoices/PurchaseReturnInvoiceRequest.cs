using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.Shard.Contract.Invoices;

public class PurchaseReturnInvoiceRequest
{
    public int PurchaseInvoiceId { get; set; }
    public int SupplierId { get; set; }
    public string Notes { get; set; }
    //public string InvoiceNumber { get; set; }
    //public string Barcode { get; set; }
    public List<PaymentRequest> Payments { get; set; }
    public List<PurchaseReturnInvoiceItemRequest> Items { get; set; }
};
