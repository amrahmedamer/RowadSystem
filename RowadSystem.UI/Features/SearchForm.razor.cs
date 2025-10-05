using Microsoft.AspNetCore.Components;

namespace RowadSystem.UI.Features;

public partial class SearchForm
{
    [Inject]
    private NavigationManager navigationManager { get; set; }

    private string searchValue = string.Empty;

     private void HandleSearch(ChangeEventArgs e)
    {
        searchValue = e.Value?.ToString() ?? string.Empty;
        if (!string.IsNullOrEmpty(searchValue))
        {
            navigationManager.NavigateTo($"/?searchValue={searchValue}");
        }
        else
        {
            navigationManager.NavigateTo("/");  
        }
    }

    private void ClearSearch()
    {
        searchValue = string.Empty;
        navigationManager.NavigateTo("/");
    }

    private void OnSearchFocus()
    {
        // Can be used for future enhancements like showing search suggestions
    }

    private void OnSearchBlur()
    {
        // Can be used for future enhancements
    }

    private string GetPlaceholderText()
    {
        // Responsive placeholder text based on screen size
        return "Search products...";
    }
}

