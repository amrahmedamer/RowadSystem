

using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Products;
using System.Buffers;

namespace RowadSystem.UI.Features.Products;

public partial class Products : ComponentBase
{
    [Inject]
    private IProductService _productService { get; set; } = default!;

    public PaginatedListResponse<ProductResponse> ProductList { get; set; } = new();
    public RequestFilters filters { get; set; } = new();

    private int CurrentPage = 1;
    private int PageSize = 10; // Default page size
    private bool IsLoading = true;
    private string SearchTerm { get; set; } = string.Empty;


    private CancellationTokenSource _debounceCancellationTokenSource = new CancellationTokenSource();

    private async Task OnSearchChangedAsync(ChangeEventArgs e)
    {
        SearchTerm = e.Value?.ToString() ?? string.Empty;

        _debounceCancellationTokenSource.Cancel();
        _debounceCancellationTokenSource.Dispose();

        _debounceCancellationTokenSource = new CancellationTokenSource();

        await Task.Delay(1000);

        if (!_debounceCancellationTokenSource.Token.IsCancellationRequested)
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                filters.SearchValue = SearchTerm.Trim();
            }
            else
            {
                filters.SearchValue = string.Empty;
            }
            await LoadProducts();
            StateHasChanged();
        }
    }
   

    protected override async Task OnInitializedAsync()
    {
        await LoadProducts();
    }


    private async Task LoadProducts()
    {
        IsLoading = true;

        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;


        ProductList = await _productService.GetProductsAsync(filters);

        IsLoading = false;
        StateHasChanged();
    }

    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            PageSize = size;
            CurrentPage = 1;
            await LoadProducts();
        }
    }

    private async Task ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= ProductList.TotalPages)
        {
            CurrentPage = pageNumber;
            await LoadProducts();
        }
    }


    //private async Task OnPageNumberChanged(ChangeEventArgs e)
    //{
    //    if (int.TryParse(e.Value?.ToString(), out var pageNumber))
    //    {
    //        if (pageNumber >= 1 && pageNumber <= ProductList.TotalPages)
    //        {
    //            CurrentPage = pageNumber;
    //            await LoadProducts();
    //        }
    //    }
    //}
}

