using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Helpers;
using System.Net.Http;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Categories;

public class CategoryService(IHttpClientFactory httpClient) : ICategoryService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");

    public async Task<Result<PaginatedListResponse<CategoryResponse>>> GetAllCategoryAsync(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/Categories?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PaginatedListResponse<CategoryResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<PaginatedListResponse<CategoryResponse>>(response);
        }

    }
    public async Task<HttpResponseMessage> AddCategory(CategoryRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Categories", request);

        return response;
    }
}
