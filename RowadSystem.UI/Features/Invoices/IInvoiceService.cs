using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Products;
using RowadSystem.Shard.Contract.Units;

namespace RowadSystem.UI.Features.Invoices;

public interface IInvoiceService
{
    Task<Result<List<UnitResponse>>> GetUnitsAsync();
    Task<Result<PurchaseInvoiceResponse>> GetPurchaseInvoiceByInvoiceNumberAsync(string invoiceNumber);
    Task<Result<SalesInvoiceResponse>> GetSalesInvoiceByInvoiceNumberAsync(string invoiceNumber);
    Task<Result<SalesReturnInvoiceResponse>> GetSalesReturnInvoiceByInvoiceNumberAsync(string invoiceNumber);
    Task<Result<PurchaseReturnInvoiceResponse>> GetPurchaseReturnInvoiceByInvoiceNumberAsync(string invoiceNumber);
    Task<Result<ProductByBarcodeDto>> GetProductBybarcodeAsync(string barcode);
    Task<Result<ProductByBarcodeForInvoiceSalesDto>> GetProductBybarcodeForInvoiceSalesAsync(string barcode);
    Task<Result<HttpResponseMessage>> SalesInvoiceAsync(SalesInvoiceRequest request);
    Task<Result<HttpResponseMessage>> SalesInvoiceReturnAsync(SalesReturnInvoiceRequest request);
    Task<Result<HttpResponseMessage>> PurchaseInvoiceAsync(PurchaseInvoiceRequest request);
    Task<Result<HttpResponseMessage>> PurchaseInvoiceReturnAsync(PurchaseReturnInvoiceRequest request);
    Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllSalesInvoiceAsync(RequestFilters requestFilters);
    Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllSalesReturnInvoiceAsync(RequestFilters requestFilters);
    Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllPurchaseInvoiceAsync(RequestFilters requestFilters);
    Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllPurchaseReturnInvoiceAsync(RequestFilters requestFilters);
    Task<Result<byte[]>> GetSalesInvoicePdfAsync(string invoiceNumber);
    Task<Result<byte[]>> GetSalesReturnInvoicePdfAsync(string invoiceNumber);
    Task<Result<byte[]>> GetPurchaseInvoicePdAsync(string invoiceNumber);
    Task<Result<byte[]>> GetPurchaseReturnInvoicePdfAsync(string invoiceNumber);
    Task<Result<HttpResponseMessage>> AddPaymentToInvoiceAsync(int invoiceId, PaymentRequest paymentRequests);


}
