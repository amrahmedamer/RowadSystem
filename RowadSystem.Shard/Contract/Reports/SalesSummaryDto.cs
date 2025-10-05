

namespace RowadSystem.Shard.Contract.Reports;
public class SalesSummaryDto
{
    public DateTime Date {  get; set; }
    public decimal TotalSales {  get; set; }
    public int TotalInvoices {  get; set; }
    public string DateLabel => Date.ToString("MM-dd");
}
