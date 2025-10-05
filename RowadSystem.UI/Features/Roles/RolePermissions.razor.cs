using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Roles;

namespace RowadSystem.UI.Features.Roles;

public partial class RolePermissions
{
    [Inject]
    private IRoleService RoleService { get; set; } = default!;
    [Parameter] public string roleId { get; set; }
    private List<PermissionGroup> permissionGroups = new();
    private bool isLoading = true;
    private RoleRequest roleToEdit = new RoleRequest();
    //private string roleName;

    protected override async Task OnInitializedAsync()
    {
        var roleResult = await RoleService.GetRoleByIdAsync(roleId);
        if (roleResult.IsSuccess)
        {
            //roleName = roleResult.Value.Name;
            var existingPermissions = roleResult.Value.Permissions.ToList();

            var modules = Enum.GetNames(typeof(Module)).ToList(); ;

            permissionGroups = modules.Select(m => new PermissionGroup
            {
                Module = m,
                CanView = existingPermissions.Contains($"{m}.View"),
                CanCreate = existingPermissions.Contains($"{m}.Create"),
                CanEdit = existingPermissions.Contains($"{m}.Edit"),
                CanDelete = existingPermissions.Contains($"{m}.Delete"),
            }).ToList();
        }

        isLoading = false;
    }

    private async Task SavePermissions()
    {
        var selectedPermissions = new List<string>();

        foreach (var pg in permissionGroups)
        {
            if (pg.CanView) selectedPermissions.Add($"{pg.Module}.View");
            if (pg.CanCreate) selectedPermissions.Add($"{pg.Module}.Create");
            if (pg.CanEdit) selectedPermissions.Add($"{pg.Module}.Edit");
            if (pg.CanDelete) selectedPermissions.Add($"{pg.Module}.Delete");
        }

        var request = new PermissionRequest
        {
            Permissions = selectedPermissions
        };

        var result = await RoleService.UpdateRolePermissionsAysnc(roleId, request);

        if (result.IsSuccess)
        {
            Navigation.NavigateTo("/admin/roles");
        }
        else
        {
            // TODO: عرض رسالة خطأ
        }
    }
}
