using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RowadSystem.Shard.Contract.Acounts;
using RowadSystem.UI.Features.Auth;

namespace RowadSystem.UI.Features.Account;

public partial class Profile
{
    [Inject]
    private IProfileService ProfileService { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    private ILocalStorageService  _localStorage { get; set; } = default!;
    [Inject]
    private CustomAuthenticationStateProvider _customAuthenticationStateProvider { get; set; } = default!;
    private UserPorfileResponse userProfile = new ();
    private bool showProfileDropdown = false;
    protected override async Task OnInitializedAsync()
    {
        var isAuthenticated = (await _customAuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        if (isAuthenticated)
        {
            userProfile = await ProfileService.GetUserProfileAsync();

        }
        else
        {
            // يمكن إظهار رسالة أو إعادة توجيه المستخدم إلى صفحة الدخول
            NavigationManager.NavigateTo("/login");
        }
        //userProfile = await ProfileService.GetUserProfileAsync();
        //Console.WriteLine("is active " + userProfile.IsDeleted);
        //StateHasChanged();
    }

    private void ToggleProfileDropdown()
    {
        showProfileDropdown = !showProfileDropdown;
    }

    private void EditProfile()
    {
        // Add the logic to edit the profile (navigate to another page or open modal)
    }

    private async Task LogOutAsync()
    {
        // Log out the user and clear authentication state
        await _localStorage.RemoveItemAsync("access_token");
        await _customAuthenticationStateProvider.MarkUserAsLoggedOutAsync();
        //_authenticationStateProvider.MarkUserAsLoggedOut();

        // Optionally redirect to the login page
        NavigationManager.NavigateTo("/login");
    }
    



}
