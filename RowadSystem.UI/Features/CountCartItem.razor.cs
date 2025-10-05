using Microsoft.AspNetCore.Components;
using RowadSystem.UI.Features.ShoppingCart;

namespace RowadSystem.UI.Features;

public partial class CountCartItem
{
    [Inject]
    private ICartService _cartService { get; set; } = default!;
    public static event Action<int>? OnCountChanged;
    private int itemCount = 0;
    protected override void OnInitialized()
    {
        itemCount =  _cartService.CountItem().Result.Value;
        //OnCountChanged?.Invoke(itemCount);
        Console.WriteLine("itemCount in CountCartItem: " + itemCount);
        StateHasChanged();
    }

}
