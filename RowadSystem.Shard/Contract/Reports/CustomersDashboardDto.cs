

namespace RowadSystem.Shard.Contract.Reports;
public class CustomersDashboardDto
{
    public int TotalCustomers { get; set; }
    public int NewCustomersThisMonth { get; set; }
    public int ActiveCustomers { get; set; }
    public int InActiveCustomers { get; set; }

    public List<CustomerSummaryDto> LatestCustomers { get; set; } = new();
}
