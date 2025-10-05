using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Auth;
using RowadSystem.UI.Features.ShoppingCart;

namespace RowadSystem.UI.Features.Auth;

public partial class Login
{

    [Inject]
    private IAuthService AuthService { get; set; } = default!;
    [Inject]
    private ITokenStorageService _tokenStorageService { get; set; } = default!;
    [Inject]
    private CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    private ICartService _cartService { get; set; } = default!;
    [Inject]
    private  AlertService _alertService { get; set; } = default!;
    private LoginRequest _loginRequest = new();
    private string? _errorMessage;
    private async Task HandleLogin()
    {
        var result = await AuthService.LoginAsync(_loginRequest);
        if (result.IsSuccess && result.Value is not null)
        {

            await _tokenStorageService.SaveTokenAsync(result.Value.Token, result.Value.RefreshToken, result.Value.Expiration);
            AuthenticationStateProvider.MarkUserAsAuthenticated(result.Value.Token);
            NavigationManager.NavigateTo("/Shop");
            var countResult = await _cartService.CountItem();
            ShoopingCart.TriggerCountChanged(countResult.Value);

        }
        else
        {

            _alertService.ShowErrorAlert(result.Error.description);
        }

    }
   
}
