using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Customers;

public class CustomerService(IHttpClientFactory httpClient) : ICustomerService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");


    public async Task<PaginatedListResponse<CustomerResponse>> GetAllCustomer(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/Customers?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        var customer = await response.Content.ReadFromJsonAsync<PaginatedListResponse<CustomerResponse>>();
        return customer ?? new PaginatedListResponse<CustomerResponse>();
    }
    public async Task<Result<PaginatedListResponse<AccountStatementDto>>> GetAccountStatement(int id, RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/Customers/account-statement/{id}?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
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
    public async Task<HttpResponseMessage> AddCustomer(CustomerRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Customers", request);

        return response;
    }
    public async Task<Result<List<CustomerResponseDto>>> GetCustomersAsync()
    {
        var response = await _httpClient.GetAsync("/api/Customers/without-filter");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<CustomerResponseDto>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<List<CustomerResponseDto>>(response);
        }

    }
}
