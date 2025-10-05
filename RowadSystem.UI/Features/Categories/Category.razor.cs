using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.UI.Features.Categories;

public partial class Category
{
    [Inject]
    private ICategoryService _categoryService { get; set; } = default!;

    private PaginatedListResponse<CategoryResponse>?  _listResponse=new();
    public RequestFilters filters { get; set; } = new();
    private bool IsLoading = true;
    private int CurrentPage = 1;
    private int PageSize = 10;
    private CategoryRequest  _categoryRequest = new();

    private Modal modal;

    private void OpenModal()
    {
        if (modal != null)
        {
            modal.Show();
        }
        else
        {
            Console.WriteLine("Modal reference is null.");
        }
    }

    private void CloseModal()
    {
        if (modal != null)
        {
            modal.Hide();
        }
    }

    private async Task HandleSubmit()
    {
        var response = await _categoryService.AddCategory(_categoryRequest);
        if (response.IsSuccessStatusCode)
        {
            CloseModal();
            await LoadCategory();
            StateHasChanged();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("❌ Error: " + error);
        }

    }
    protected override async Task OnInitializedAsync()
    {
        await LoadCategory();
    }

    private async Task LoadCategory()
    {
        IsLoading = true;

        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;

        var result = await _categoryService.GetAllCategoryAsync(filters);
        _listResponse = result.Value;
        IsLoading = false;
        StateHasChanged();


    }
    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            PageSize = size;
            CurrentPage = 1;
            await LoadCategory();
        }
    }

    private async Task ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= _listResponse.TotalPages)
        {
            CurrentPage = pageNumber;
            await LoadCategory();
        }
    }
}
