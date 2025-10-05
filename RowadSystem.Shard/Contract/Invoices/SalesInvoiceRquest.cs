using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.Shard.Contract.Invoices;

public class SalesInvoiceRequest
{

    public int? OrderId { get; set; }
    public int? CustomerId { get; set; }
    public string? CouponCode { get; set; }
    public string? Notes { get; set; }
    public List<PaymentRequest> Payments { get; set; } = new();
    public List<SalesInvoiceItemRequest> Items { get; set; }= new();
};
//public record SalesInvoiceRequest(

//    int? OrderId,
//    int? CustomerId,
//    string? CouponCode,
//    string? Notes,
//    List<PaymentRequest> Payments,
//    List<SalesInvoiceItemRequest> Items
//);
