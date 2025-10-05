using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Discounts;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DiscountsController(IDiscountService discountService) : ControllerBase
{
    private readonly IDiscountService _discountService = discountService;

    [HttpPost("")]
    public async Task<IActionResult> AddDiscount([FromBody] DiscountRequest request)
    {
        var result = await _discountService.AddDiscount(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDiscount(int id, [FromBody] DiscountRequest request)
    {
        var result = await _discountService.UpdateDiscount(id, request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("assign-to-product")]
    public async Task<IActionResult> AssignDiscount([FromBody] AssignDiscountToProductRequest request)
    {
        var result = await _discountService.AssignDiscountToProduct(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _discountService.GetAllDiscounts();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

}
