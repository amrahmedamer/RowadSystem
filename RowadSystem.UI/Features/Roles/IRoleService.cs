using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Roles;

namespace RowadSystem.UI.Features.Roles;

public interface IRoleService
{
    Task<Result<List<RoleRespons>>> GetRolesAsync();
    Task<Result<RoleRespons>> GetRoleByIdAsync(string roleId);
    Task<Result> UpdateRoleAysnc(string roleId, RoleRequest request);
    Task<Result> UpdateRolePermissionsAysnc(string roleId, PermissionRequest request);
    Task<Result> AddRoleAysnc(RoleRequest request);

}
