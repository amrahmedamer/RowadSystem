using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Users;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync([FromQuery] RequestFilters requestFilters)
    {
        var result = await _userService.GetAllAsync(requestFilters);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        var result = await _userService.GetByIdAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> AddAsync([FromBody] UserRequest request)
    {
        var result = await _userService.AddUserAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UdateAsync([FromRoute] string id, [FromBody] UserRequest request)
    {
        var result = await _userService.UpdateUserAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("toggle-status/{id}")]
    public async Task<IActionResult> ToggleStatusAsync([FromRoute] string id)
    {
        var result = await _userService.ToggleStatus(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("unlook-user/{id}")]
    public async Task<IActionResult> UnLookUserAsync([FromRoute] string id)
    {
        var result = await _userService.UnLookUser(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
