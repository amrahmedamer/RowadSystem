using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Suppliers;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SuppliersController(ISupplierService supplierService) : ControllerBase
{
    private readonly ISupplierService _supplierService = supplierService;

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] SupplierRequest request)
    {
        var result = await _supplierService.AddSupplierAsync(User.GetUserId(), request);
        return result.IsSuccess ? RedirectToAction(nameof(GetById), null, new { id = result.Value }) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _supplierService.GetSupplierByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("account-supplier/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccountStatementAsync([FromRoute] int id, [FromQuery] RequestFilters request)
    {
        var result = await _supplierService.GetAccountStatementAsync(id, request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters request)
    {
        var result = await _supplierService.GetAllSuppliersAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("without-filter")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _supplierService.GetAllSuppliersWithoutFilterAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
