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


    

}

