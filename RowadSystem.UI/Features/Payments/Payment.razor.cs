using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.ShoppingCarts;
using System.Text.Json;


namespace RowadSystem.UI.Features.Payments;

public partial class Payment
{
    [Inject] public IPaymentService PaymentService { get; set; }=default!;
    [Parameter]
    public string CheckoutData { get; set; } 
    public CheckoutResponse CheckoutResponse { get; set; } = new();

    private PaymentMethodOrder selectedPaymentMethod = PaymentMethodOrder.CreditCard;
   
    private bool isCheckoutCompleted = false;
 
    private List<ShoppingCartResponse> shoppingCartItems = new();

    protected override async Task OnInitializedAsync()
    {
        await GetShoppingCartItems();
        // فك تشفير الـ JSON وتحويله إلى الكائن CheckoutResponse
        var checkoutResponse = JsonSerializer.Deserialize<CheckoutResponse>(Uri.UnescapeDataString(CheckoutData));
        if (checkoutResponse != null)
        {
            CheckoutResponse = checkoutResponse;
        }
    }

    private void SelectPaymentMethod(PaymentMethodOrder paymentMethod)
    {
        selectedPaymentMethod = paymentMethod;
    }

    private async Task GetShoppingCartItems()
    {
        var result = await PaymentService.GetCartItem();
        if (result.IsSuccess && result.Value != null)
            shoppingCartItems = result.Value;
    }


}
