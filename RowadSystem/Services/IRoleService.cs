using RowadSystem.Shard.Contract.Roles;

namespace RowadSystem.Services;

public interface IRoleService
{
    Task<Result<List<RoleRespons>>> GetRolesAsync(bool includeDisabled = false);
    Task<Result<RoleRespons>> GetRoleByIdAsync(string Id);
    Task<Result> AddRoleAysnc(RoleRequest request);
    Task<Result> UpdateRoleAysnc(string id, RoleRequest request);
    Task<Result> UpdateRolePermissionAysnc(string RoleId, PermissionRequest request);
    Task<Result> ToggleStatusAsync(string id);
    Task<Result<List<string>>> GetAllRolesAsync();
}
