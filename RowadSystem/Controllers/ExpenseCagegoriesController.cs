using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Services;
using RowadSystem.Shard.Contract.ExpenseCategory;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ExpenseCagegoriesController(IExpenseCategoryService categoryService) : ControllerBase
{
    private readonly IExpenseCategoryService _categoryService = categoryService;
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters request)
    {
        var result = await _categoryService.GetExpenseCategoryAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("without-filter")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryService.GetExpenseCategoryAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] ExpenseCategoryRequest request)
    {
        var result = await _categoryService.AddExpenseCategoryAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
