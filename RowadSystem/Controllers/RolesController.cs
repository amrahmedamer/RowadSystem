using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Roles;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync([FromQuery] bool includeDisabled = false)
    {
        var result = await _roleService.GetRolesAsync(includeDisabled);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("without-filter")]
    public async Task<IActionResult> GetAllRolesAsync()
    {
        var result = await _roleService.GetAllRolesAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var result = await _roleService.GetRoleByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> AddAsync([FromBody] RoleRequest request)
    {
        var result = await _roleService.AddRoleAysnc(request);
        return result.IsSuccess ? Ok(result) : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] RoleRequest request)
    {
        var result = await _roleService.UpdateRoleAysnc(id, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("{id}/permissions")]
    public async Task<IActionResult> UpdateRolePermissionsAsync([FromRoute] string id, [FromBody]PermissionRequest request)
    {
        var result = await _roleService.UpdateRolePermissionAysnc(id, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("toggle-status/{id}")]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id)
    {
        var result = await _roleService.ToggleStatusAsync(id);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
