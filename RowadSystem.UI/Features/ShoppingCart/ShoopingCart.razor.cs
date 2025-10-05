using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.ShoppingCarts;

namespace RowadSystem.UI.Features.ShoppingCart;

public partial class ShoopingCart
{
    [Inject]
    private ICartService _cartService { get; set; } = default!;

    public List<ShoppingCartItemRequest> Items { get; set; } = new();
    public List<ShoppingCartResponse> shoppingCartResponses = new();
    private decimal _total;
    public static event Action<int>? OnCountChanged;

    protected override async Task OnInitializedAsync()
    {
        await LoadItems();
    }
    // Method to trigger the event
    public static void TriggerCountChanged(int count)
    {
        OnCountChanged?.Invoke(count);
    }
    public async Task LoadItems()
    {
        shoppingCartResponses = await _cartService.GetCartItem();
        OnCountChanged?.Invoke(shoppingCartResponses.Count);
        _total = shoppingCartResponses.Select(x => x.TotalShoppingCart).FirstOrDefault();

    }
    public async Task RemoveItem(int itemId)
    {
        var result = await _cartService.RemoveItem(itemId);

        if (result != null)
            Console.WriteLine("deleted");

        await LoadItems();

        StateHasChanged();
    }
    private async Task IncreaseQuantity(int itemId)
    {
        var item = shoppingCartResponses.FirstOrDefault(x => x.Id == itemId);
        if (item.Quantity >= 1)
        {
            item.Quantity++;
            await _cartService.UpdateQuantityCartItem(new UpdateQuantityRequest { itemId = item.Id, quantity = item.Quantity });

            await LoadItems();
            StateHasChanged();
        }

    }

    private async Task DecreaseQuantity(int itemId)
    {
        var item = shoppingCartResponses.FirstOrDefault(x => x.Id == itemId);
        if (item.Quantity > 1)
        {

            item.Quantity--;
            await _cartService.UpdateQuantityCartItem(new UpdateQuantityRequest { itemId = item.Id, quantity = item.Quantity });
            await LoadItems();
            StateHasChanged();
        }
    }
}
