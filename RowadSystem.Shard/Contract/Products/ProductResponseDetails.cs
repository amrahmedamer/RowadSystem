using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Discounts;

namespace RowadSystem.Shard.Contract.Products;

public class ProductResponseDetails
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public decimal? PurchasePrice { get; set; } = 0.0m;
    public string? Description { get; set; } = string.Empty;
    public decimal? Quantity { get; set; } = 0;
    public IList<ImageRequest>? Images { get; set; } = new List<ImageRequest>();
    public DiscountDto? Discount { get; set; } = new DiscountDto();
    public List<ProductUnitDto>? ProductUnit { get; set; } = new List<ProductUnitDto>();

    public string Status =>
        Quantity > 0 ? ProductStatus.Active.ToString()
        : Quantity == 0 ? ProductStatus.OutOfStock.ToString()
        : ProductStatus.Inactive.ToString();

    public decimal? FinalPrice =>
        Discount is not null
            ? (Discount.IsPercentage == true
                ? PurchasePrice - (PurchasePrice * (Discount.Value ?? 0) / 100)
                : PurchasePrice - (Discount.Value ?? 0))
            : PurchasePrice ?? 0;


}
