using ApexCharts;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.ExpenseCategory;
using RowadSystem.Shard.Contract.Helpers;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.ExpensesCategory;

public class ExpenseCategoryService(IHttpClientFactory httpClient) : IExpenseCategoryService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");

    async Task<Result<PaginatedListResponse<ExpenseCategoryResponse>>> IExpenseCategoryService.GetAllExpenseCategoryAsync(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/ExpenseCagegories?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<ExpenseCategoryResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<ExpenseCategoryResponse>>(response);
        }

    }
    public async Task<HttpResponseMessage> AddExpenseCagegory(ExpenseCategoryRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/ExpenseCagegories", request);

        return response;
    }

   
}