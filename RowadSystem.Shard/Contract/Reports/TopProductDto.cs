using System;


namespace RowadSystem.Shard.Contract.Reports;
public class TopProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
    public decimal TotalSales { get; set; }
}
