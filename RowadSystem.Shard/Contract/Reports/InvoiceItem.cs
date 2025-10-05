

namespace RowadSystem.Shard.Contract.Reports;
public class InvoiceItem
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
