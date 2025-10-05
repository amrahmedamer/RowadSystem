using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Invoices;

namespace RowadSystem.UI.Features.Invoices.Purchase;

public partial class InvoiceDetail
{
    [Inject]
    private IInvoiceService InvoiceService { get; set; } = default!;
    [Parameter]
    public string InvoiceNumber { get; set; }
    private PurchaseInvoiceResponse _invoiceResponse;

    protected override async Task OnInitializedAsync()
    {
        var result = await InvoiceService.GetPurchaseInvoiceByInvoiceNumberAsync(InvoiceNumber);

        _invoiceResponse = result.Value;
    }
}
