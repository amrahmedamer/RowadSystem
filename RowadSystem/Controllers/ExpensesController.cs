using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Entity;
using RowadSystem.API.Services;
using RowadSystem.Shard.Contract.Expenses;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ExpensesController(IExpenseService expenseService) : ControllerBase
{
    private readonly IExpenseService _expenseService = expenseService;
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters request)
    {
        var result = await _expenseService.GetExpenseAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] ExpenseRequest request)
    {
        var result = await _expenseService.AddExpenseAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
