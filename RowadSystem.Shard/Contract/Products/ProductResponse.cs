using RowadSystem.Shard.Contract.Image;

namespace RowadSystem.Shard.Contract.Products;

public class ProductResponse
{
    public int Id { get; set; }
   public string Name { get; set; }
    //decimal PurchasePrice,
    public string? Description { get; set; }
    public decimal Quantity { get; set; }
    public string CategoryName { get; set; }
    public string BrandName { get; set; }
    public ProductUnitDto ProductUnits { get; set; }
    public List<ImageResponse>? Images { get; set; }

};
