using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpGet("")]
    public async Task<IActionResult> GetCategories([FromQuery]RequestFilters request)
    {
        var result = await _categoryService.GetCategoriesAsync(request);
        return result.IsSuccess ? Ok(result.Value  ) : result.ToProblem();

    }
    [HttpGet("without-filter")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _categoryService.GetCategoriesAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }
    [HttpPost("")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
    {
        var result = await _categoryService.AddCategoryAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

   

}
