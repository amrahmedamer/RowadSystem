

namespace RowadSystem.Shard.Contract.Reports;
public class InvoiceDetail
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Paid { get; set; }
    public decimal Due { get; set; }
    public List<InvoiceItem> Items { get; set; } = new();
}
