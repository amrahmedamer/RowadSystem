using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Expenses;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.API.Services;

public interface IExpenseService
{
    Task<Result<PaginatedListResponse<ExpenseResponse>>> GetExpenseAsync(RequestFilters requestFilters);
    Task<Result> AddExpenseAsync(ExpenseRequest request);
}
