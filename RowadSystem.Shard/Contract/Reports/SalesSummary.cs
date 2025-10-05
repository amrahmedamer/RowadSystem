

namespace RowadSystem.Shard.Contract.Reports;
public class SalesSummary
{
    public DateTime Date { get; set; }
    public decimal TotalSales { get; set; }

    public string FormattedDate => Date.ToString("yyyy-MM-dd");
}
