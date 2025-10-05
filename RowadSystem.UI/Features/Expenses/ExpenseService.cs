using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.ExpenseCategory;
using RowadSystem.Shard.Contract.Expenses;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;
using System.Net.Http;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Expenses;

public class ExpenseService(IHttpClientFactory httpClient) : IExpenseService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");

    public async Task<Result<PaginatedListResponse<ExpenseResponse>>> GetAllExpenseAsync(RequestFilters filters)
    {

        var response = await _httpClient.GetAsync($"/api/Expenses?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<ExpenseResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<ExpenseResponse>>(response);
        }

    }
    public async Task<HttpResponseMessage> AddExpense(ExpenseRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Expenses", request);

        return response;
    }
    public async Task<Result<List<ExpenseCategoryResponse>>> GetExpenseCategoryAsync()
    {
        var response = await _httpClient.GetAsync($"/api/ExpenseCagegories/without-filter");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<ExpenseCategoryResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<List<ExpenseCategoryResponse>>(response);
        }
    }
}
