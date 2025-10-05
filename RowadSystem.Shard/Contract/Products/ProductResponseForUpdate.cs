using RowadSystem.Shard.Contract.Image;


namespace RowadSystem.Shard.Contract.Products;
public class ProductResponseForUpdate
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? DiscountId { get; set; }
    public int? BrandId { get; set; }
    public int CategoryId { get; set; }
    public List<ImageResponse>? Images { get; set; } = new();
}
