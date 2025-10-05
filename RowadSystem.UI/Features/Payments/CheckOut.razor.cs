using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Orders;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Payments;

public partial class CheckOut
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    private IPaymentService PaymentService   { get; set; } = default!;

    private bool isProcessing = false;

    private async Task HandlePayment()
    {
        try
        {
            isProcessing = true;
            StateHasChanged();  // Refresh the UI to show processing message

            // 1. Call your API to create payment on Paymob
            //var paymentUrl = await PaymentService.AddPayment();

            //if (paymentUrl != null)
            //{
            //    // 2. Redirect to Paymob payment page
            //    NavigationManager.NavigateTo(paymentUrl);
            //}
            //else
            //{
            //    // Handle error
            //    Console.WriteLine("Error creating payment.");
            //}
        }
        catch (Exception ex)
        {
            // Handle any exception that occurs during payment creation
            Console.WriteLine($"Error during payment process: {ex.Message}");
        }
        finally
        {
            isProcessing = false;
            StateHasChanged();  // Refresh the UI after process completion
        }
    }
}
