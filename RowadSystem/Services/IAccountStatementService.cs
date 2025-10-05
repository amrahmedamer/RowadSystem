using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Reports;
using InvoiceDetail = RowadSystem.Shard.Contract.Reports.InvoiceDetail;

namespace RowadSystem.API.Services;

public interface IAccountStatementService
{
    //Task<Result<InvoiceSummary>> GetDailySalesAsync();
    //Task<Result<InvoiceSummary>> GetDailyPurchaseAsync();
    //Task<Result<InvoiceSummary>> GetDailySalesReturnAsync();
    //Task< Result<InvoiceSummary>> GetDailyPurchaseReturnAsync();

    Task<Result<InvoiceSummary>> GetSalesSummary();
    //Task<Result<List<InvoiceDetail>>> GetSalesDetails();
    Task<Result<PaginatedListResponse<InvoiceDetail>>> GetSalesDetails(RequestFilters requestFilters);

    Task<Result<InvoiceSummary>> GetPurchaseSummary();
    Task<Result<PaginatedListResponse<InvoiceDetail>>> GetPurchaseDetails(RequestFilters requestFilters);

    Task<Result<InvoiceSummary>> GetSalesReturnSummary();
    Task<Result<PaginatedListResponse<InvoiceDetail>>> GetSalesReturnDetails(RequestFilters requestFilters);

    Task<Result<InvoiceSummary>> GetPurchaseReturnSummary();
    Task<Result<PaginatedListResponse<InvoiceDetail>>> GetPurchaseReturnDetails(RequestFilters requestFilters);

}
