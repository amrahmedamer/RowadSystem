using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.ExpenseCategory;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.UI.Features.ExpensesCategory;

public interface IExpenseCategoryService
{
    Task<Result<PaginatedListResponse<ExpenseCategoryResponse>>> GetAllExpenseCategoryAsync(RequestFilters filters);
    Task<HttpResponseMessage> AddExpenseCagegory(ExpenseCategoryRequest request);
}
