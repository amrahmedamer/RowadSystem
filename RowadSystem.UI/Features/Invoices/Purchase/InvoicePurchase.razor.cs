using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;


namespace RowadSystem.UI.Features.Invoices.Purchase;

public partial class InvoicePurchase
{

    [Inject]
    private IInvoiceService InvoiceService { get; set; } = default!;

    private PaginatedListResponse<InvoiceResponse>? invoices;
    public RequestFilters filters { get; set; } = new();
    private bool IsLoading = true;
    private int CurrentPage = 1;
    private int PageSize = 10;

    protected override async Task OnInitializedAsync()
    {
        await LoadInvoices();
    }

    private async Task LoadInvoices()
    {
        IsLoading = true;

        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;

        var result = await InvoiceService.GetAllPurchaseInvoiceAsync(filters);
        invoices = result.Value;
        IsLoading = false;


    }
    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            PageSize = size;
            CurrentPage = 1;
            await LoadInvoices();
        }
    }

    private async Task ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= invoices.TotalPages)
        {
            CurrentPage = pageNumber;
            await LoadInvoices();
        }
    }


}
