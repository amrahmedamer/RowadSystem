using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;
using RowadSystem.Shard.Contract.Suppliers;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Suppliers;

public class SupplierService(IHttpClientFactory httpClient) : ISupplierService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");


    public async Task<PaginatedListResponse<SupplierResponse>> GetAllSupplier(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/Suppliers?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        var suppliers = await response.Content.ReadFromJsonAsync<PaginatedListResponse<SupplierResponse>>();
        return suppliers ?? new PaginatedListResponse<SupplierResponse>();
    }
    public async Task<Result<List<SupplierResponse>>> GetSuppliersAsync()
    {
        var response = await _httpClient.GetAsync("/api/Suppliers/without-filter");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<SupplierResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<List<SupplierResponse>>(response);
        }

    }
    public async Task<HttpResponseMessage> AddSupplier(SupplierRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Suppliers", request);

        return response;
    }
    public async Task<Result<PaginatedListResponse<AccountStatementDto>>> GetAccountStatement(int id, RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/Suppliers/account-supplier/{id}?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<AccountStatementDto>>();
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<AccountStatementDto>>(response);
        }
    }
}
