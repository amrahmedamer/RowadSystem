using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.UI.Features.Addresses;

namespace RowadSystem.UI.Features.Payments;

public partial class CashOnDelivery
{

    [Inject]
    private IPaymentService PaymentService { get; set; } = default!;
    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;

    private async Task HandleOrderConfirmation()
    {
        var result = await PaymentService.UpdatePaymentMethodOrder(PaymentMethodOrder.CashOnDelivery);
        if (result.IsSuccess)
        {
            _navigationManager.NavigateTo("/order-confirmation");
        }

    }
}
