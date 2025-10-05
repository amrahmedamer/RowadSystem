using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.ExpenseCategory;
using RowadSystem.Shard.Contract.Expenses;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.UI.Features.Expenses;

public interface IExpenseService
{
    Task<Result<PaginatedListResponse<ExpenseResponse>>> GetAllExpenseAsync(RequestFilters filters);
    Task<Result<List<ExpenseCategoryResponse>>> GetExpenseCategoryAsync();

    Task<HttpResponseMessage> AddExpense(ExpenseRequest request);
}
