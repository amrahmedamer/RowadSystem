using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Paymob;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Payments;

public partial class Pay
{
    [Inject]
    private IPaymentService PaymentService { get; set; } = default!;
    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;
    private PaymentData paymentData = new PaymentData();
    private string? iframeUrl;
    [Parameter]
    public CheckoutResponse CheckoutResponse { get; set; } = new();
    //private int orderId;
    //private decimal amount;

    //private async Task Start()
    //{
    //    var payload = new StartPaymentRequest
    //    {
    //        AmountCents = 10000,
    //        Currency = "EGP",
    //        MerchantOrderId = Guid.NewGuid().ToString("N"),
    //        Billing = new BillingData { FirstName = "John", LastName = "Doe", Email = "user@example.com", City = "Cairo", Country = "EG" }

    //    };

    //    var res = await PaymentService.Card(payload);
    //    iframeUrl = res!.IframeUrl;
    //}
    private async Task Start()
    {
        //var response = await PaymentService.ProcessToCheckOutAsync();
        //if (response.IsSuccess)
        //{
           


            var payload = new StartPaymentRequest
            {
                AmountCents = (int)(CheckoutResponse.TotalAmount * 100), // 100 EGP
                Currency = "EGP",
                MerchantOrderId = CheckoutResponse.OrderId.ToString(),
                Billing = new BillingData
                {
                    FirstName = paymentData.FirstName,
                    LastName = paymentData.LastName,
                    Email = paymentData.Email,
                    City = paymentData.City,
                    Country = paymentData.Country
                }
            };
            var res = await PaymentService.Card(payload);
            if (res != null)
            {
                iframeUrl = res.IframeUrl;
            }
            else
            {
                iframeUrl = null;
                Console.WriteLine("فشل في إنشاء رابط الدفع");
            }
        //}
        //else
        //{
        //    Console.WriteLine("فشل في استرجاع تفاصيل السلة");
        //}

    }

    private sealed class StartPaymentResponse
    {
        public string IframeUrl { get; set; } = "";
        public long OrderId { get; set; }
    }



    private class PaymentData
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }


}
