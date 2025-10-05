namespace RowadSystem.Shard.Contract.Products;

public class ProductRequest
{
    public string Name { get; set; } = null!;
    //public decimal PurchasePrice { get; set; }
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public int? DiscountId { get; set; }
    public int? BrandId { get; set; }
    public int CategoryId { get; set; }
    //public int Quantity { get; set; }
    //public int SupplierId { get; set; }
    public List<UnitWithConversionDto> UnitsWithConversion { get; set; } = new();
    public List<string>? Images { get; set; }= new();
}
