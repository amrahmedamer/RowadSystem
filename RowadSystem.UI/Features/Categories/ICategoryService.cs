using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.UI.Features.Categories;

public interface ICategoryService
{
    Task<Result<PaginatedListResponse<CategoryResponse>>> GetAllCategoryAsync(RequestFilters filters);
    Task<HttpResponseMessage> AddCategory(CategoryRequest request);
}
