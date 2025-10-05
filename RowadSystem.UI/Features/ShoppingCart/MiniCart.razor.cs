using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.ShoppingCarts;

namespace RowadSystem.UI.Features.ShoppingCart;

public partial class MiniCart
{
    [Inject]
    private ICartService _cartService { get; set; } = default!;

    public List<ShoppingCartItemRequest> Items { get; set; } = new();
    void Remove(int id) => _cartService.RemoveItem(id);
}
