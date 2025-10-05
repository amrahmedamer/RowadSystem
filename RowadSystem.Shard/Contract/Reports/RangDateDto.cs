

namespace RowadSystem.Shard.Contract.Reports;
public class RangDateDto
{
    public DateTime StartDate { get; set; }= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
    public DateTime EndDate { get; set; }=  new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                                    .AddMonths(1)
                                    .AddDays(-1);
}
