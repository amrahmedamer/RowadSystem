using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Reports;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.AccountStatements;

public class AccountStatementService(IHttpClientFactory httpClient): IAccountStatementService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");
    public async Task<Result<InvoiceSummary>> GetSalesSummary()
    {
        var response = await _httpClient.GetAsync($"/api/AccountStatements/sales/summary");
        var result = await response.Content.ReadFromJsonAsync<InvoiceSummary>();
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<InvoiceSummary>(response);
        }
    }
    public async Task<Result<PaginatedListResponse<InvoiceDetail>>> GetSalesDetails(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/AccountStatements/sales/details?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<InvoiceDetail>>();
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<InvoiceDetail>>(response);
        }
    }
    //public async Task<Result<List<InvoiceDetail>>> GetSalesDetails()
    //{
    //    var response = await _httpClient.GetAsync($"/api/AccountStatements/sales/details");
    //    var result = await response.Content.ReadFromJsonAsync<List<InvoiceDetail>>();
    //    if (response.IsSuccessStatusCode)
    //    {
    //        return Result.Success(result);
    //    }
    //    else
    //    {
    //        return await PorblemDetailsExtensions.HandleErrorResponse<List<InvoiceDetail>>(response);
    //    }
    //}
    public async Task<Result<InvoiceSummary>> GetPurchaseSummary()
    {
        var response = await _httpClient.GetAsync($"/api/AccountStatements/purchase/summary");
        var result = await response.Content.ReadFromJsonAsync<InvoiceSummary>();
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<InvoiceSummary>(response);
        }
    }
    public async Task<Result<PaginatedListResponse<InvoiceDetail>>> GetPurchaseDetails(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/AccountStatements/purchase/details?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<InvoiceDetail>>();
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<InvoiceDetail>>(response);
        }
    }
    public async Task<Result<InvoiceSummary>> GetSalesReturnSummary()
    {
        var response = await _httpClient.GetAsync($"/api/AccountStatements/sales-return/summary");
        var result = await response.Content.ReadFromJsonAsync<InvoiceSummary>();
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<InvoiceSummary>(response);
        }
    }
    public async Task<Result<PaginatedListResponse<InvoiceDetail>>> GetSalesReturnDetails(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/AccountStatements/sales-return/details?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<InvoiceDetail>>();
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<InvoiceDetail>>(response);
        }
    }
    public async Task<Result<InvoiceSummary>> GetPurchaseReturnSummary()
    {
        var response = await _httpClient.GetAsync($"/api/AccountStatements/purchase-return/summary");
        var result = await response.Content.ReadFromJsonAsync<InvoiceSummary>();
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<InvoiceSummary>(response);
        }
    }
    public async Task<Result<PaginatedListResponse<InvoiceDetail>>> GetPurchaseReturnDetails(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/AccountStatements/purchase-return/details?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<InvoiceDetail>>();
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<InvoiceDetail>>(response);
        }
    }

    //public async Task<Result<InvoiceSummary>> GetSalesSummary() => await Get<InvoiceSummary>("api/AccountStatements/sales/summary");
    //public async Task<Result<List<InvoiceDetail>>> GetSalesDetails() => await Get<List<InvoiceDetail>>("api/AccountStatements/sales/details");

    //public async Task<Result<InvoiceSummary>> GetPurchaseSummary() => await Get<InvoiceSummary>("api/AccountStatements/purchase/summary");
    //public async Task<Result<List<InvoiceDetail>>> GetPurchaseDetails() => await Get<List<InvoiceDetail>>("api/AccountStatements/purchase/details");

    //public async Task<Result<InvoiceSummary>> GetSalesReturnSummary() => await Get<InvoiceSummary>("api/AccountStatements/sales-return/summary");
    //public async Task<Result<List<InvoiceDetail>>> GetSalesReturnDetails() => await Get<List<InvoiceDetail>>("api/AccountStatements/sales-return/details");

    //public async Task<Result<InvoiceSummary>> GetPurchaseReturnSummary() => await Get<InvoiceSummary>("api/AccountStatements/purchase-return/summary");
    //public async Task<Result<List<InvoiceDetail>>> GetPurchaseReturnDetails() => await Get<List<InvoiceDetail>>("api/AccountStatements/purchase-return/details");

    //private async Task<Result<T>> Get<T>(string url)
    //{
    //    try
    //    {
    //        var response = await _httpClient.GetFromJsonAsync<T>(url);
    //        return Result.Success(response!);


    //    }
    //    catch (Exception ex)
    //    {
    //        return await PorblemDetailsExtensions.HandleErrorResponse<T>(ex);
    //    }
    //}
}
