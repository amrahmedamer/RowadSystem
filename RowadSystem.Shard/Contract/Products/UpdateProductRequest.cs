

namespace RowadSystem.Shard.Contract.Products;
public class UpdateProductRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? DiscountId { get; set; }
    public int? BrandId { get; set; }
    public int CategoryId { get; set; }
    public List<string>? Images { get; set; } = new();
}
