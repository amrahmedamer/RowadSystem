

using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.Shard.Contract.Invoices;
public class SalesReturnInvoiceResponse
{
    public int SalesReturnInvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = null!;
    public DateTime InvoiceDate { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public List<InvoiceItemResponse> Items { get; set; } = new List<InvoiceItemResponse>();
    public List<PaymentResponse> Payments { get; set; } = new();
}
