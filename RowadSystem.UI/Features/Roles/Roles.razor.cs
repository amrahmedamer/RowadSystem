using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Roles;

namespace RowadSystem.UI.Features.Roles;

public partial class Roles
{
    [Inject]
    private IRoleService RoleService { get; set; } = default!;
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;
    private List<RoleRespons> roles = new();
    private CreateRole? CreateComponent ;
    private UpdateRole? UpdateRoleComponent ;
  

    protected override async Task OnInitializedAsync()
    {
       await LoadRoles();
        StateHasChanged();
       
    }
    private async Task LoadRoles()
    {

        var result = await RoleService.GetRolesAsync();
        if (result.IsSuccess)
        {
            roles = result.Value;
            //await InvokeAsync(StateHasChanged);

        }
    }

    private void CreateRole()
    {
        CreateComponent.OpenModal();
    }
    private async Task EditRoleComponent(string RoleId)
    {
          UpdateRoleComponent.RoleId = RoleId;
          await UpdateRoleComponent.OpenModalAsync();
     
    }
   
   
    private void RolePermission(string roleId)
    {
        Navigation.NavigateTo($"/admin/roles/{roleId}/permissions");
    }

    private async Task DeleteRole(string roleId)
    {
        //    var result = await RoleService.DeleteRoleAsync(roleId);
        //    if (result.IsSuccess)
        //    {
        //        roles.RemoveAll(r => r.Id == roleId);
        //    }
    }
    private async Task OnRoleCreatedOrUpdated()
    {
        await LoadRoles(); 
        StateHasChanged(); 
    }
}
