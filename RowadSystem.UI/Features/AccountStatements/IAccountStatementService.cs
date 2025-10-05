using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Reports;

namespace RowadSystem.UI.Features.AccountStatements;

public interface IAccountStatementService
{
    Task<Result<InvoiceSummary>> GetSalesSummary();
    //Task<Result<List<InvoiceDetail>>> GetSalesDetails();
    Task<Result<PaginatedListResponse<InvoiceDetail>>> GetSalesDetails(RequestFilters filters);
    Task<Result<InvoiceSummary>> GetPurchaseSummary();
    Task<Result<PaginatedListResponse<InvoiceDetail>>> GetPurchaseDetails(RequestFilters filters);
    Task<Result<InvoiceSummary>> GetSalesReturnSummary();
    Task<Result<PaginatedListResponse<InvoiceDetail>>> GetSalesReturnDetails(RequestFilters filters);
    Task<Result<InvoiceSummary>> GetPurchaseReturnSummary();
    Task<Result<PaginatedListResponse<InvoiceDetail>>> GetPurchaseReturnDetails(RequestFilters filters);
}
