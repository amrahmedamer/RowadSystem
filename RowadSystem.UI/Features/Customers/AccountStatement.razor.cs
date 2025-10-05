using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;
using System.Data;

namespace RowadSystem.UI.Features.Customers;

public partial class AccountStatement
{
    [Inject]
    private ICustomerService customerService { get; set; } = default!;
    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;
    [Inject]
    private AlertService AlertService { get; set; } = default!;
    private PaginatedListResponse<AccountStatementDto> _listResponse = new();
    private List<CustomerResponseDto> _customerResponses = new();
    private int Id { get; set; }
    public RequestFilters filters { get; set; } = new();
    private int CurrentPage = 1;
    private int PageSize = 10;
    private bool IsLoading = true;



    protected override async Task OnInitializedAsync()
    {
        var result = await customerService.GetCustomersAsync();
        if (result.IsSuccess)
            _customerResponses = result.Value;
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

            var result = await customerService.GetAccountStatement(id, filters);

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


    private async Task OnCustomerSelected(int value)
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
    private void NavigateToInvoiceDetails(string invoiceNumber)
    {
        _navigationManager.NavigateTo($"/invoice-sales-details/{invoiceNumber}");
    }
}


