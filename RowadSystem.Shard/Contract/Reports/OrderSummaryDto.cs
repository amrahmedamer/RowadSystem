

using RowadSystem.Shard.consts.Enums;

namespace RowadSystem.Shard.Contract.Reports;
public class OrderSummaryDto
{
    public int OrderId { get; set; }
    public string UserName { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
}
