using RowadSystem.API.Entity;
using RowadSystem.API.Errors;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.ExpenseCategory;
using RowadSystem.Shard.Contract.Expenses;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.API.Services;

public class ExpenseCategoryService(ApplicationDbContext context) : IExpenseCategoryService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PaginatedListResponse<ExpenseCategoryResponse>>> GetExpenseCategoryAsync(RequestFilters requestFilters)
    {
        var response = _context.ExpenseCategories
            .Select(x => new ExpenseCategoryResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            });

        if (response == null)
            return Result.Failure<PaginatedListResponse<ExpenseCategoryResponse>>(ExpenseErrors.NotFound);

        var result = await PaginatedList<ExpenseCategoryResponse>.CreatePaginationAsync(response, requestFilters.PageNumber, requestFilters.PageSize);

        return Result.Success(result.Adapt<PaginatedListResponse<ExpenseCategoryResponse>>());
    }
    public async Task<Result<List<ExpenseCategoryResponse>>> GetExpenseCategoryAsync()
    {
        var response = await _context.ExpenseCategories
            .Select(x => new ExpenseCategoryResponse
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

        if (response == null)
            return Result.Failure<List<ExpenseCategoryResponse>>(ExpenseErrors.NotFound);


        return Result.Success(response);
    }
    public async Task<Result> AddExpenseCategoryAsync(ExpenseCategoryRequest request)
    {

        var expense = request.Adapt<ExpenseCategory>();

        if (expense is null)
            return Result.Failure(ExpenseErrors.InvalidCategory);

        await _context.AddAsync(expense);
        await _context.SaveChangesAsync();


        return Result.Success();
    }
}
