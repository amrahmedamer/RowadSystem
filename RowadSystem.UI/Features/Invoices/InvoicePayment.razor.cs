using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.UI.Features.Invoices;

public partial class InvoicePayment
{
    [Inject]
    private IInvoiceService _invoiceService { get; set; } = default!;
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;
    //[Parameter] 
    public int InvoiceId { get; set; }
    //public  string InvoiceNumber { get; set; }
    private PaymentRequest newPayment = new PaymentRequest();

    private Modal modal;
    public void OpenModal()
    {
        modal.Show();
    }
    public void CloseModal()
    {
        modal.Hide();
    }
    private async Task HandleAddPayment()
    {
         var result = await _invoiceService.AddPaymentToInvoiceAsync(InvoiceId, newPayment);

         if (result.IsSuccess)
         {
            CloseModal();
            StateHasChanged();
         }
         else
         {
             Console.WriteLine("فشل إضافة الدفع.");
         }
    }

}
