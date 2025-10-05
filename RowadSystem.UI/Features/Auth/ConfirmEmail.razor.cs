using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Auth;
using System.Threading.Tasks;

namespace RowadSystem.UI.Features.Auth;

public partial class ConfirmEmail
{
    [Inject]
    private IAuthService AuthService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    [SupplyParameterFromQuery]
    public string? Email { get; set; }
    private OtpRequest _otpRequest { get; set; } = new();

    private ConfirmEmailRequest _confirmEmailRequest { get; set; } = new();


    private string? _errorMessage;

    private async Task ConfirmEmailAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(_confirmEmailRequest.Code))
        {
            // Handle invalid input
            return;
        }
        _confirmEmailRequest.Email = Email;

        try
        {
            await AuthService.ConfirmEmailAsync(_confirmEmailRequest);
            NavigationManager.NavigateTo("/Home");
        }
        catch (Exception ex)
        {
            _errorMessage = ex.Message;
            // Handle error (e.g., show error message)
        }
    }

    private async Task ResendOtp()
    {
        var result = _otpRequest.Email = Email!;
        await AuthService.ResendOtpAsync(_otpRequest);
        NavigationManager.NavigateTo("/Confirm-email");

    }
}
