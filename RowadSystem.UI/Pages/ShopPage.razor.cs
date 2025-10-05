using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Products;
using RowadSystem.Shard.Contract.ShoppingCarts;
using RowadSystem.UI.Features.Products;
using RowadSystem.UI.Features.ShoppingCart;

namespace RowadSystem.UI.Pages;

public partial class ShopPage
{
    [Inject]
    private IProductService _productService { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    NavigationManager navigationManager { get; set; } = default!;
    [Inject]
    private ICartService _cartService { get; set; } = default!;
    //public static event Action<int>? OnCountChanged;

    public PaginatedListResponse<ProductResponse> ProductList { get; set; } = new();
    public RequestFilters filters { get; set; } = new();
    [Parameter]
    [SupplyParameterFromQuery]
    public string? searchValue { get; set; }

    private int CurrentPage = 1;
    private int PageSize = 12;
    private bool IsLoading = true;
    private List<ShoppingCartItemRequest> _cartItems = new List<ShoppingCartItemRequest>();

    public IReadOnlyList<ShoppingCartItemRequest> CartItems => _cartItems;

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(searchValue))
        {
            filters.SearchValue = searchValue;
        }
        else
        {
            filters.SearchValue = string.Empty;
        }

        await LoadProducts();
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {

        await LoadProducts();

    }
    private void RedirectToLogin()
    {
        NavigationManager.NavigateTo("/login");
    }

    private async Task LoadProducts()
    {
        IsLoading = true;


        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;

        ProductList = await _productService.GetProductsAsync(filters);


        StateHasChanged();
    }

    public async Task AddToCart(ProductResponse product)
    {
            var existingItem = _cartItems.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                await _cartService.AddCartItem(existingItem);
            }
            else
            {
                var newItem = new ShoppingCartItemRequest
                {
                    ProductId = product.Id,
                    Quantity = 1,
                    UnitId = product.ProductUnits.Id
                };
                _cartItems.Add(newItem);
                await _cartService.AddCartItem(newItem);

                var countResult = await _cartService.CountItem();
               ShoopingCart.TriggerCountChanged( countResult.Value);

                //navigationManager.NavigateTo("/Cart");
                StateHasChanged();
            }
            {
                Console.WriteLine("Product is null");
                return;
            }
        
    }

    private void ShoopingCart_OnCountChanged(int obj)
    {
        throw new NotImplementedException();
    }
}
