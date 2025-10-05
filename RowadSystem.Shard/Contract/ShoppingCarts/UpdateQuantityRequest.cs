

namespace RowadSystem.Shard.Contract.ShoppingCarts;
public class UpdateQuantityRequest
{
    public int cartId { get; set; }
    public int itemId { get; set; }
    public int quantity { get; set; }
}
