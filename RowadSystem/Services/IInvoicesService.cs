using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.Services;

public interface IInvoicesService
{
    Task<Result> AddSalesInvoiceAsync(string createByUserId, SalesInvoiceRequest request);
    Task<Result> AddSalesReturnInvoiceAsync(string createByUserId, SalesReturnInvoiceRequest request);
    Task<Result> AddPurchaseInvoiceAsync(string createByUserId, PurchaseInvoiceRequest request);
    Task<Result> AddPurchaseReturnInvoiceAsync(string createByUserId, PurchaseReturnInvoiceRequest request);
    Task<Result<PurchaseInvoiceResponse>> GetPurchaseInvoicesByInvoiceNumber(string invoiceNumber);
    Task<Result<SalesInvoiceResponse>> GetSalesInvoicesByInvoiceNumber(string invoiceNumber);
    Task<Result<SalesReturnInvoiceResponse>> GetSalesReturnInvoicesByInvoiceNumber(string invoiceNumber);
    Task<Result<PurchaseReturnInvoiceResponse>> GetPurchaseReturnInvoicesByInvoiceNumber(string invoiceNumber);
    Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllSalesInvoices(RequestFilters requestFilters);
    Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllSalesReturnInvoices(RequestFilters requestFilters);
    Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllPurchaseInvoices(RequestFilters requestFilters);
    Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllPurchaseReturnInvoices(RequestFilters requestFilters);
    Task<Result> AddPaymentToInvoiceAsync(int invoiceId, PaymentRequest paymentRequests);




}
