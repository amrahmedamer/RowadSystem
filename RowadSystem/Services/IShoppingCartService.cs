using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Orders;
using RowadSystem.Shard.Contract.ShoppingCarts;

namespace RowadSystem.Services;

public interface IShoppingCartService
{
    Task<Result<ShoppingCart>> GetOrCreateCartAsync(ShoppingCartRequest request);
    Task<Result> AddItemToCartAsync(int cartId, ShoppingCartItemRequest request);
    Task<Result> RemoveItemFromCartAsync(ShoppingCartRequest request, int itemId);
    Task<Result> UpdateItemQuantityAsync(int cartId, int itemId, int quantity);
    Task<Result<IEnumerable<ShoppingCartResponse>>> GetCartItemsAsync(ShoppingCartRequest request);
    Task<Result<CheckoutResponse>> CheckoutCartAsync(ShoppingCartRequest request, OrderRequest orderRequest);
    //Task<Result> CheckoutCartAsync(ShoppingCartRequest request, AddressRequest address);
    Task<Result> ClearCartAsync(int cartId);
    Task<Result<int>> CountCartItem(int cartId);

}
