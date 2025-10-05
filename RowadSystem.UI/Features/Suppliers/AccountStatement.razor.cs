using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;
using RowadSystem.Shard.Contract.Suppliers;

namespace RowadSystem.UI.Features.Suppliers;

public partial class AccountStatement
{
    [Inject]
    private ISupplierService _supplierService { get; set; } = default!;
    [Inject]
    private AlertService AlertService { get; set; } = default!;
    private PaginatedListResponse<AccountStatementDto> _listResponse = new();
    private List<SupplierResponse>  SupplierResponse = new();
    private int Id { get; set; }
    public RequestFilters filters { get; set; } = new();
    private int CurrentPage = 1;
    private int PageSize = 10;
    private bool IsLoading = true;



    protected override async Task OnInitializedAsync()
    {
        var result = await _supplierService.GetSuppliersAsync();
        if (result.IsSuccess)
            SupplierResponse = result.Value;
        else
            AlertService.ShowErrorAlert(result.Error.description);

    }

    private async Task LoadAccountStatementAsync(int id)
    {
        IsLoading = true;

        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;
        Id = id;
        if (Id > 0)
        {

            var result = await _supplierService.GetAccountStatement(id, filters);

            if (result.IsSuccess)
                _listResponse = result.Value;
            else
                AlertService.ShowErrorAlert(result.Error.description);
        }
        else
        {
            _listResponse = new();
        }

        IsLoading = false;


    }


    private async Task OnSupplierSelected(int value)
    {
        await LoadAccountStatementAsync(value);
    }


    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            PageSize = size;
            CurrentPage = 1;
            await LoadAccountStatementAsync(Id);
        }
    }

    private async Task ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= _listResponse.TotalPages)
        {
            CurrentPage = pageNumber;
            await LoadAccountStatementAsync(Id);
        }
    }
}


