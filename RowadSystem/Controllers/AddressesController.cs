using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Services;

namespace RowadSystem.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AddressesController(IAddressService addressService) : ControllerBase
{
    private readonly IAddressService _addressService = addressService;

    [HttpGet("Governrates")]
    public async Task<IActionResult> GetAllGovernrate()
    {
        var result = await _addressService.GetAllGovernrate();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("Cities")]
    public async Task<IActionResult> GetAllCities()
    {
        var result = await _addressService.GetAllCities();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
