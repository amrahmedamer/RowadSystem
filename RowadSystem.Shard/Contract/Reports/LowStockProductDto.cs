

namespace RowadSystem.Shard.Contract.Reports;
public class LowStockProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
