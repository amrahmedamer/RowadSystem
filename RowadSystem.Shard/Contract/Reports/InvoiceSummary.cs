
namespace RowadSystem.Shard.Contract.Reports;
public  class InvoiceSummary
{
    public int InvoiceCount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalDue => TotalAmount - TotalPaid;
}
