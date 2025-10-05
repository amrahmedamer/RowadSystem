using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Coupons;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CouponsController(ICouponService couponService) : ControllerBase
{
    private readonly ICouponService _couponService = couponService;

    [HttpPost("")]
    public async Task<IActionResult> AddCoupon([FromBody] CouponRequest request)
    {
        var result = await _couponService.AddCoupon(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
