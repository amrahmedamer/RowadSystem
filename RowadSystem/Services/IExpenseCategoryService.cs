using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.ExpenseCategory;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.API.Services;

public interface IExpenseCategoryService
{
    Task<Result> AddExpenseCategoryAsync(ExpenseCategoryRequest request);
    Task<Result<PaginatedListResponse<ExpenseCategoryResponse>>> GetExpenseCategoryAsync(RequestFilters requestFilters);
    Task<Result<List<ExpenseCategoryResponse>>> GetExpenseCategoryAsync();
}
