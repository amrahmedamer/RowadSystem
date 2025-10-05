using RowadSystem.Shard.Contract.Image;
using RowadSystem.Shard.Contract.Products;

namespace RowadSystem.Shard.Contract.ShoppingCarts;

public class ShoppingCartResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int? UnitId { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public decimal TotalShoppingCart { get; set; }
    public string ProductName { get; set; }
    public List<ImageResponse>? ImageResponse { get; set; }

};
