using RowadSystem.Shard.consts.Enums;


namespace RowadSystem.Shard.Contract.Products;
public class ProductByBarcodeDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int BaseUnitId { get; set; }
    public decimal BaseUnitPrice { get; set; }
    public string BaseUnitName { get; set; }
    public string Status { get; set; }
}
