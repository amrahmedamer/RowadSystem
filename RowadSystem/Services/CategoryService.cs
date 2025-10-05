using RowadSystem.API.Errors;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.Services;

public class CategoryService(ApplicationDbContext context) : ICategoryService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddCategoryAsync(CategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name
        };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<List<CategoryResponse>>> GetCategoriesAsync()
    {
        var categories = await _context.Categories
            .Select(c => new CategoryResponse
            (
                 c.Id,
                 c.Name
            ))
            .ToListAsync();

        //if (categories is null)
        //    return Result.Failure<List<CategoryResponse>>(CategoryErrors.NotFound);

        return Result.Success(categories);
    }
    public async Task<Result<PaginatedListResponse<CategoryResponse>>> GetCategoriesAsync(RequestFilters requestFilters)
    {
        var categories =  _context.Categories
            .Select(c => new CategoryResponse
            (
                 c.Id,
                 c.Name
            ));

        var result = await PaginatedList<CategoryResponse>.CreatePaginationAsync(categories, requestFilters.PageNumber, requestFilters.PageSize);

        if (!result.Items.Any())
           return Result.Failure<PaginatedListResponse<CategoryResponse>>(CategoryErrors.NotFound);

       return Result.Success(result.Adapt<PaginatedListResponse<CategoryResponse>>());
    }
}
