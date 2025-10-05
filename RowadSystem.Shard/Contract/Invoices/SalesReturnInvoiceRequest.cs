using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.Shard.Contract.Invoices;

//public record SalesReturnInvoiceRequest
//(
//    int? CustomerId,
//    string Notes,
//    int SalesInvoiceId,
//    string? UserId,
//    List<PaymentRequest> Payments,
//    ICollection<SalesReturnItemRequest> Items
//);


public class SalesReturnInvoiceRequest
{

    public int? CustomerId { get; set; }

    //public string InvoiceNumber { get; set; } = null!;
    //public string Barcode { get; set; } = null!;
    public string Notes { get; set; } = null!;
    public int SalesInvoiceId { get; set; }
    public string? UserId { get; set; }
    public List<PaymentRequest> Payments { get; set; } = new List<PaymentRequest>();
    public ICollection<SalesReturnItemRequest> Items { get; set; } = new List<SalesReturnItemRequest>();


}
