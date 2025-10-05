using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Paymob;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Payments;

public partial class Wallet
{
    [Inject]
    private IPaymentService PaymentService { get; set; } = default!;
    [Inject]
    private NavigationManager  NavigationManager { get; set; } = default!;
    [Parameter]
    public CheckoutResponse CheckoutResponse { get; set; } = new();
    private string msisdn = "";

    private async Task StartWallet()
    {
        var payload = new StartWalletPaymentRequest
        {
            AmountCents = (int)(CheckoutResponse.TotalAmount * 100), // 100 EGP
            Currency = "EGP",
            MerchantOrderId = CheckoutResponse.OrderId.ToString(),
            WalletMsisdn = msisdn,
            Billing = new BillingData { FirstName = "John", LastName = "Doe", Email = "user@example.com", Phone = msisdn, City = "Cairo", Country = "EG" }
            
        };

        var res = await PaymentService.Wallet(payload);
        if (!string.IsNullOrWhiteSpace(res!.RedirectUrl))
            NavigationManager.NavigateTo(res.RedirectUrl!, forceLoad: true);
    }

    private sealed class StartWalletResponse { public string? RedirectUrl { get; set; } }
}
