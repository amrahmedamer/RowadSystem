using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Products;
using RowadSystem.Shard.Contract.Units;
using System.Net.Http.Json;


namespace RowadSystem.UI.Features.Invoices;

public class InvoiceService(IHttpClientFactory httpClient) : IInvoiceService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");

    public async Task<Result<List<UnitResponse>>> GetUnitsAsync()
    {
        var response = await _httpClient.GetAsync("/api/Units");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<UnitResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<List<UnitResponse>>(response);
        }

    }


    public async Task<Result<ProductByBarcodeDto>> GetProductBybarcodeAsync(string barcode)
    {

        var response = await _httpClient.GetAsync($"/api/products/barcode/{barcode}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ProductByBarcodeDto>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<ProductByBarcodeDto>(response);
        }



    }
    public async Task<Result<ProductByBarcodeForInvoiceSalesDto>> GetProductBybarcodeForInvoiceSalesAsync(string barcode)
    {

        var response = await _httpClient.GetAsync($"/api/products/barcode-invoice-sales/{barcode}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ProductByBarcodeForInvoiceSalesDto>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<ProductByBarcodeForInvoiceSalesDto>(response);
        }


    }
    public async Task<Result<PurchaseInvoiceResponse>> GetPurchaseInvoiceByInvoiceNumberAsync(string invoiceNumber)
    {

        var response = await _httpClient.GetAsync($"/api/Invoices/purchase/{invoiceNumber}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PurchaseInvoiceResponse>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PurchaseInvoiceResponse>(response);
        }

    }
    public async Task<Result<SalesInvoiceResponse>> GetSalesInvoiceByInvoiceNumberAsync(string invoiceNumber)
    {

        var response = await _httpClient.GetAsync($"/api/Invoices/sales/{invoiceNumber}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<SalesInvoiceResponse>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<SalesInvoiceResponse>(response);
        }

    }
    public async Task<Result<SalesReturnInvoiceResponse>> GetSalesReturnInvoiceByInvoiceNumberAsync(string invoiceNumber)
    {

        var response = await _httpClient.GetAsync($"/api/Invoices/sales-return/{invoiceNumber}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<SalesReturnInvoiceResponse>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<SalesReturnInvoiceResponse>(response);
        }

    }
    public async Task<Result<PurchaseReturnInvoiceResponse>> GetPurchaseReturnInvoiceByInvoiceNumberAsync(string invoiceNumber)
    {

        var response = await _httpClient.GetAsync($"/api/Invoices/purchase-return/{invoiceNumber}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PurchaseReturnInvoiceResponse>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PurchaseReturnInvoiceResponse>(response);
        }

    }
    public async Task<Result<HttpResponseMessage>> SalesInvoiceAsync(SalesInvoiceRequest request)
    {

        var response = await _httpClient.PostAsJsonAsync("/api/Invoices/sales", request);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(response);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<HttpResponseMessage>(response);
        }
        //if (response.IsSuccessStatusCode)
        //{
        //    return new ProductResponseDetails();
        //}
        //else
        //{
        //    var errorText = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine($"Error: {errorText}");

        //    return new ProductResponseDetails();

        //}

    }

    public async Task<Result<HttpResponseMessage>> SalesInvoiceReturnAsync(SalesReturnInvoiceRequest request)
    {

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Invoices/sales-return", request);

            if (response.IsSuccessStatusCode)
            {
                return Result.Success(response);
            }
            else
            {
                return await PorblemDetailsExtensions.HandleErrorResponse<HttpResponseMessage>(response);
            }
        }
        catch (Exception ex)
        {

            return Result.Failure<HttpResponseMessage>(new Error("Exception", ex.Message, 500));
        }
    }

    public async Task<Result<HttpResponseMessage>> PurchaseInvoiceAsync(PurchaseInvoiceRequest request)
    {

        var response = await _httpClient.PostAsJsonAsync("/api/Invoices/purchase", request);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(response);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<HttpResponseMessage>(response);
        }

    }
    public async Task<Result<HttpResponseMessage>> PurchaseInvoiceReturnAsync(PurchaseReturnInvoiceRequest request)
    {

        var response = await _httpClient.PostAsJsonAsync("/api/Invoices/purchase-return", request);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(response);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<HttpResponseMessage>(response);
        }

    }

    public async Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllSalesInvoiceAsync(RequestFilters filters)
    {

        var response = await _httpClient.GetAsync($"/api/Invoices/sales?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<InvoiceResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<InvoiceResponse>>(response);
        }

    }
    public async Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllSalesReturnInvoiceAsync(RequestFilters filters)
    {

        var response = await _httpClient.GetAsync($"/api/Invoices/sales-return?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<InvoiceResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<InvoiceResponse>>(response);
        }

    }
    public async Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllPurchaseInvoiceAsync(RequestFilters filters)
    {

        var response = await _httpClient.GetAsync($"/api/Invoices/purchase?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<InvoiceResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<InvoiceResponse>>(response);
        }

    }
    public async Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllPurchaseReturnInvoiceAsync(RequestFilters filters)
    {

        var response = await _httpClient.GetAsync($"/api/Invoices/purchase-return?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<InvoiceResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<InvoiceResponse>>(response);
        }

    }
    public async Task<Result<byte[]>> GetSalesInvoicePdfAsync(string invoiceNumber)
    {
        var response = await _httpClient.GetAsync($"/api/Invoices/print-sales/{invoiceNumber}");

        if (response.IsSuccessStatusCode)
        {
            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            return Result.Success(pdfBytes);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<byte[]>(response);
        }
    }
    public async Task<Result<byte[]>> GetSalesReturnInvoicePdfAsync(string invoiceNumber)
    {
        var response = await _httpClient.GetAsync($"/api/Invoices/print-sales-return/{invoiceNumber}");

        if (response.IsSuccessStatusCode)
        {
            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            return Result.Success(pdfBytes);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<byte[]>(response);
        }
    }
    public async Task<Result<byte[]>> GetPurchaseInvoicePdAsync(string invoiceNumber)
    {
        var response = await _httpClient.GetAsync($"/api/Invoices/print-purchase/{invoiceNumber}");

        if (response.IsSuccessStatusCode)
        {
            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            return Result.Success(pdfBytes);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<byte[]>(response);
        }
    }
    public async Task<Result<byte[]>> GetPurchaseReturnInvoicePdfAsync(string invoiceNumber)
    {
        var response = await _httpClient.GetAsync($"/api/Invoices/print-purchase-return/{invoiceNumber}");

        if (response.IsSuccessStatusCode)
        {
            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            return Result.Success(pdfBytes);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<byte[]>(response);
        }
    }


    public async Task<Result<HttpResponseMessage>> AddPaymentToInvoiceAsync(int invoiceId, PaymentRequest request)
    {

        var response = await _httpClient.PostAsJsonAsync($"/api/Invoices/{invoiceId}/update-invoice-payment", request);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(response);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<HttpResponseMessage>(response);
        }

    }
}
