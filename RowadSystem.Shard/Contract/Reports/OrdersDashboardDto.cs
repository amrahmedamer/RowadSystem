

namespace RowadSystem.Shard.Contract.Reports;
public class OrdersDashboardDto
{
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int CancelledOrders { get; set; }
    public List<OrderSummaryDto> LatestOrders { get; set; }
}
