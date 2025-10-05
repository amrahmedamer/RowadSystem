using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Invoices;

namespace RowadSystem.UI.Features.Invoices.Sales;

public partial class InvoiceDetail
{
    [Inject]
    private IInvoiceService InvoiceService { get; set; } = default!;
    [Parameter]
    public string InvoiceNumber { get; set; } = null!;
    private SalesInvoiceResponse _salesInvoiceResponse = new();
    private InvoicePayment invoicePaymentComponent = default!;

    private void OpenPaymentModal()
    {
        invoicePaymentComponent.OpenModal();
    }
    protected override async Task OnInitializedAsync()
    {

        var result = await InvoiceService.GetSalesInvoiceByInvoiceNumberAsync(InvoiceNumber);

        _salesInvoiceResponse = result.Value;
        invoicePaymentComponent.InvoiceId = _salesInvoiceResponse.SalesInvoiceId;
        //invoicePaymentComponent.InvoiceNumber = _salesInvoiceResponse.InvoiceNumber;



    }
}
