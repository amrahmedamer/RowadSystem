using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Brands;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BrandsController(IBrandService brandService) : ControllerBase
{
    private readonly IBrandService _brandService = brandService;
    [HttpPost("")]
    public async Task<IActionResult> AddBrandAsync([FromBody] BrandRequset requset)
    {

        var result = await _brandService.AddBrandAsync(requset);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllBrandsAsync()
    {
        var result = await _brandService.GetAllBrandsAsync();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
