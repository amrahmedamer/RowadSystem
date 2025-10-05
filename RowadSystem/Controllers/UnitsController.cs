using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Services;

namespace RowadSystem.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UnitsController(IUnitService unitService) : ControllerBase
{
    private readonly IUnitService _unitService = unitService;

    [HttpGet("")]
    public async Task<IActionResult> GetUnits()
    {
        var result = await _unitService.GetUnitsAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
