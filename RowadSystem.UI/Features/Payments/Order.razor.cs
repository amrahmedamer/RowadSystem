using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Orders;

namespace RowadSystem.UI.Features.Payments;

public partial class Order
{
    [Inject] public IPaymentService PaymentService { get; set; } = default!;
    [Inject] public NavigationManager Navigation { get; set; } = default!;
    private OrderResponse orderDetails = new();
    protected override async Task OnInitializedAsync()
    {
        var result = await PaymentService.GetOrder();
        if (result.IsSuccess && result.Value != null)
            orderDetails = result.Value;
    }
    private void GoToHome()
    {
        Navigation.NavigateTo("/");
    }
}
