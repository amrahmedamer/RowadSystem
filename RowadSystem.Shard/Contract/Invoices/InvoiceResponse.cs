using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Products;

namespace RowadSystem.Shard.Contract.Invoices;

public class InvoiceResponse
{

    public int Id { get; set; } 
    public string InvoiceNumber { get; set; } = null!;
    public DateTime InvoiceDate { get; set; }
    public string? Notes { get; set; }
    public List<InvoiceDetails> Items { get; set; } = new ();
    //public List<PaymentResponse> Payments { get; set; } = new();
    public PaymentStatus PaymentStatus { get; set; }


}
