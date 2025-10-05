
namespace RowadSystem.Shard.Contract.Reports;
public class DailyAccountStatementDto
{
    public DateTime Date { get; set; }

    public List<InvoiceSummary> InvoiceSummaries { get; set; } = new();

    //public decimal TotalSales => InvoiceSummaries.Sum(x => x.TotalSales);
    public decimal TotalPaid => InvoiceSummaries.Sum(x => x.TotalPaid);
    public decimal TotalDue => InvoiceSummaries.Sum(x => x.TotalDue);
    public decimal TotalReturns { get; set; }
    //public decimal EndingBalance => TotalSales - TotalReturns;
}
