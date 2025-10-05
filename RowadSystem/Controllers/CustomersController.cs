using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    [HttpPost("")]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerRequest request)
    {

        var result = await _customerService.AddCustomerAsync(User.GetUserId(), request);

        return result.IsSuccess ? RedirectToAction(nameof(GetById), null, new { id = result.Value }) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetCustomerByIdAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("account-statement/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccountStatementAsync([FromRoute] int id, [FromQuery] RequestFilters request)
    {
        var result = await _customerService.GetAccountStatementAsync(id, request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters request)
    {
        var result = await _customerService.GetAllCustomersAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("without-filter")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _customerService.GetAllCustomerWithoutFilterAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
