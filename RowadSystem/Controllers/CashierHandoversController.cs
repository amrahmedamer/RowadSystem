using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Services;
using RowadSystem.Shard.Contract.Cashiers;

namespace RowadSystem.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CashierHandoversController(ICashierHandoverService cashierHandoverService) : ControllerBase
{
    private readonly ICashierHandoverService _cashierHandoverService = cashierHandoverService;

    [HttpPost("handover")]
    public async Task<IActionResult> HandoverCashier([FromBody] CashierHandoverDTO handoverDTO)
    {
        var userId = User.GetUserId();
        
        var result = await _cashierHandoverService.HandoverCashierAsync(userId, handoverDTO.AmountInDrawer);

        return result.IsSuccess ? Ok(result) : result.ToProblem();
    }
   
}
