using RowadSystem.API.Entity;
using RowadSystem.API.Errors;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Expenses;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.API.Services;

public class ExpenseService(ApplicationDbContext context) : IExpenseService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PaginatedListResponse<ExpenseResponse>>> GetExpenseAsync(RequestFilters requestFilters)
    {
        var response =  _context.Expenses
            .Include(x => x.ExpenseCategory)
            .Select(x => new ExpenseResponse
            {
                Amount = x.Amount,
                ExpenseCategoryName = x.ExpenseCategory.Name,
                PaymentMethod = x.PaymentMethod,
            } );

       if( response == null )
            return Result.Failure<PaginatedListResponse<ExpenseResponse>>(ExpenseErrors.NotFound );

        var result = await PaginatedList<ExpenseResponse>.CreatePaginationAsync(response, requestFilters.PageNumber, requestFilters.PageSize);

        return Result.Success(result.Adapt<PaginatedListResponse<ExpenseResponse>>());
    }
    public async Task<Result> AddExpenseAsync(ExpenseRequest request)
    {

        var expense = request.Adapt<Expense>();

        if (expense.Amount <= 0)
            return Result.Failure(ExpenseErrors.InvalidAmount);

        if (request.ExpenseCategoryId <= 0)
            return Result.Failure(ExpenseErrors.InvalidCategory);

        await _context.AddAsync(expense);
        await _context.SaveChangesAsync();


        return Result.Success();
    }
}
