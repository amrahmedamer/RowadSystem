using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Invoices;

namespace RowadSystem.UI.Features.Invoices.PurchaseReturn;

public partial class InvoiceDetail
{
    [Inject]
    private IInvoiceService InvoiceService { get; set; } = default!;
    [Parameter]
    public string InvoiceNumber { get; set; }
    private PurchaseReturnInvoiceResponse _invoiceResponse;

    protected override async Task OnInitializedAsync()
    {
        var result = await InvoiceService.GetPurchaseReturnInvoiceByInvoiceNumberAsync(InvoiceNumber);

        _invoiceResponse = result.Value;
    }
}
