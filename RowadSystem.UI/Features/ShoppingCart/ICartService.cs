using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.ShoppingCarts;

namespace RowadSystem.UI.Features.ShoppingCart;

public interface ICartService
{
    Task<HttpResponseMessage> AddCartItem(ShoppingCartItemRequest request);
    Task<List<ShoppingCartResponse>> GetCartItem();
    Task<HttpResponseMessage> RemoveItem(int itemId);
    Task<HttpResponseMessage> UpdateQuantityCartItem(UpdateQuantityRequest request);
    Task<Result<int>> CountItem();
}
