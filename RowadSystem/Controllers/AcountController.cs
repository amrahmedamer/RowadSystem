using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Acounts;

namespace RowadSystem.Controllers;
[Route("me")]
[ApiController]
[Authorize]
public class AcountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpGet("")]
    public async Task<IActionResult> Info()
    {
        var result = await _userService.GetUserProfileAsync(User.GetUserId());
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("info")]
    public async Task<IActionResult> Info([FromBody] UpdateUserProfileRequest request)
    {
        var result = await _userService.UpdateUserProfileAsync(User.GetUserId(), request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        var result = await _userService.ChangePasswordAsync(User.GetUserId(), request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
