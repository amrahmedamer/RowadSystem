using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Helpers;
namespace RowadSystem.Services;

public interface ICategoryService
{
    Task<Result> AddCategoryAsync(CategoryRequest request);
    Task<Result<List<CategoryResponse>>> GetCategoriesAsync();
    Task<Result<PaginatedListResponse<CategoryResponse>>> GetCategoriesAsync(RequestFilters requestFilters);
}
