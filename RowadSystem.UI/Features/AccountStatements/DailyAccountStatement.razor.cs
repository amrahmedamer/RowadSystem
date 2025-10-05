//using Microsoft.AspNetCore.Components;
//using RowadSystem.Shard.Contract.Reports;

//namespace RowadSystem.UI.Features.AccountStatements;

//public partial class DailyAccountStatement
//{
//    [Inject] private IAccountStatementService _accountStatementService { get; set; } = default!;

//    [Inject]
//    private AlertService AlertService { get; set; } = default!;
//    private InvoiceSummary? sales;
//    private InvoiceSummary? purchase;
//    private InvoiceSummary? salesReturn;
//    private InvoiceSummary? purchaseReturn;

//    protected override async Task OnInitializedAsync()
//    {
//        var resultSales = await _accountStatementService.GetSales();
//        if (resultSales.IsSuccess)
//            sales = resultSales.Value;
//        else
//            AlertService.ShowErrorAlert(resultSales.Error.description);


//        var resultPurchase = await _accountStatementService.GetPurchase();
//        if (resultPurchase.IsSuccess)
//            purchase = resultPurchase.Value;
//        else
//            AlertService.ShowErrorAlert(resultPurchase.Error.description);


//        var resultSalesReturn = await _accountStatementService.GetSalesReturn();
//        if (resultSalesReturn.IsSuccess)
//            salesReturn = resultSalesReturn.Value;
//        else
//            AlertService.ShowErrorAlert(resultSalesReturn.Error.description);


//        var resultPurchaseReturn = await _accountStatementService.GetPurchaseReturn();
//        if (resultPurchaseReturn.IsSuccess)
//            purchaseReturn = resultPurchaseReturn.Value;
//        else
//            AlertService.ShowErrorAlert(resultPurchaseReturn.Error.description);
//    }


//}
using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Reports;
using System.Threading.Tasks;

namespace RowadSystem.UI.Features.AccountStatements;

public partial class DailyAccountStatement
{
    [Inject] private IAccountStatementService _accountStatementService { get; set; } = default!;
    [Inject] private NavigationManager _navigationManager{ get; set; } = default!;

    private bool IsLoading = true;

    private enum TabType { Sales, Purchase, SalesReturn, PurchaseReturn }
    private TabType currentTab = TabType.Sales;

    private InvoiceSummary? SalesSummary, PurchaseSummary, SalesReturnSummary, PurchaseReturnSummary;
    //private List<InvoiceDetail>?  PurchaseDetails, SalesReturnDetails, PurchaseReturnDetails;
    private PaginatedListResponse<InvoiceDetail>? SalesDetails, PurchaseDetails, SalesReturnDetails, PurchaseReturnDetails;

    private InvoiceSummary? CurrentSummary => currentTab switch
    {
        TabType.Sales => SalesSummary,
        TabType.Purchase => PurchaseSummary,
        TabType.SalesReturn => SalesReturnSummary,
        TabType.PurchaseReturn => PurchaseReturnSummary,
        _ => null
    };

    //private PaginatedListResponse<InvoiceDetail>? CurrentDetails => currentTab switch
    //{
    //    TabType.Sales => SalesDetails,
    //    _ => null
    //};
    private PaginatedListResponse<InvoiceDetail>? CurrentDetails => currentTab switch
    {
        TabType.Sales => SalesDetails,
        TabType.Purchase => PurchaseDetails,
        TabType.SalesReturn => SalesReturnDetails,
        TabType.PurchaseReturn => PurchaseReturnDetails,
        _ => null
    };

    public RequestFilters filters { get; set; } = new();
    private int CurrentPage = 1;
    private int PageSize = 10;

    protected override async Task OnInitializedAsync()
    {
        await LoadTabDataAsync();
        IsLoading = false;
    }

    //private async Task LoadData()
    //{
    //    var sum = await _accountStatementService.GetSalesSummary();
    //    if (sum.IsSuccess) SalesSummary = sum.Value;
    //    await LoadSalesDetailsAsync();
    //    //SalesDetails = (await AccountService.GetSalesDetails()).Value;

    //    var pSum = await _accountStatementService.GetPurchaseSummary();
    //    if (pSum.IsSuccess) PurchaseSummary = pSum.Value;
    //    PurchaseDetails = (await _accountStatementService.GetPurchaseDetails()).Value;

    //    var srSum = await _accountStatementService.GetSalesReturnSummary();
    //    if (srSum.IsSuccess) SalesReturnSummary = srSum.Value;
    //    SalesReturnDetails = (await _accountStatementService.GetSalesReturnDetails()).Value;

    //    var prSum = await _accountStatementService.GetPurchaseReturnSummary();
    //    if (prSum.IsSuccess) PurchaseReturnSummary = prSum.Value;
    //    PurchaseReturnDetails = (await _accountStatementService.GetPurchaseReturnDetails()).Value;
    //}
    private async Task LoadTabDataAsync()
    {
        IsLoading = true;

        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;

        // Fetching the details based on the selected tab
        switch (currentTab)
        {
            case TabType.Sales:
                var sum = await _accountStatementService.GetSalesSummary();
                     if (sum.IsSuccess) SalesSummary = sum.Value;
                SalesDetails = (await _accountStatementService.GetSalesDetails(filters)).Value;
                break;

            case TabType.Purchase:
                var pSum = await _accountStatementService.GetPurchaseSummary();
                   if (pSum.IsSuccess) PurchaseSummary = pSum.Value;
                PurchaseDetails = (await _accountStatementService.GetPurchaseDetails(filters)).Value;
                break;

            case TabType.SalesReturn:
                var srSum = await _accountStatementService.GetSalesReturnSummary();
                    if (srSum.IsSuccess) SalesReturnSummary = srSum.Value;
                SalesReturnDetails = (await _accountStatementService.GetSalesReturnDetails(filters)).Value;
                break;

            case TabType.PurchaseReturn:
                var prSum = await _accountStatementService.GetPurchaseReturnSummary();
                   if (prSum.IsSuccess) PurchaseReturnSummary = prSum.Value;
                PurchaseReturnDetails = (await _accountStatementService.GetPurchaseReturnDetails(filters)).Value;
                break;
        }

        IsLoading = false;
    }

    private async Task SelectTab(TabType tab)
    {
        currentTab = tab;
        await LoadTabDataAsync();

    }
    //private void SelectTab(TabType tab) => currentTab = tab;

    //private async Task LoadSalesDetailsAsync()
    //{
    //    IsLoading = true;

    //    filters.PageNumber = CurrentPage;
    //    filters.PageSize = PageSize;

    //    SalesDetails = (await _accountStatementService.GetSalesDetails(filters)).Value;

    //    IsLoading = false;


    //}
    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            PageSize = size;
            CurrentPage = 1;
            await LoadTabDataAsync();
        }
    }

    private async Task ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= SalesDetails.TotalPages)
        {
            CurrentPage = pageNumber;
            await LoadTabDataAsync();
        }
    }


    private void NavigateToInvoiceDetails(string invoiceNumber)
    {
       

        switch (currentTab)
        {
            case TabType.Sales:
                _navigationManager.NavigateTo($"/invoice-sales-details/{invoiceNumber}");
                break;

            case TabType.Purchase:
                _navigationManager.NavigateTo($"/invoice-purchase-details/{invoiceNumber}");
                break;

            case TabType.SalesReturn:
                _navigationManager.NavigateTo($"/invoice-sales-return-details/{invoiceNumber}");
                break;

            case TabType.PurchaseReturn:
                _navigationManager.NavigateTo($"/invoice-purchase-return-details/{invoiceNumber}");
                break;
        }
    }
}





