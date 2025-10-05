using Blazorise;
using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.consts;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Roles;

namespace RowadSystem.UI.Features.Roles;

public partial class CreateRole
{
    [Inject]
    private IRoleService RoleService { get; set; } = default!;
    [Inject]
    private NavigationManager  Navigation { get; set; } = default!;
  
    [Parameter]
    public EventCallback OnRoleCreated { get; set; } = default!;
    private RoleRequest newRole = new RoleRequest();
    private IList<string> allPermissions = Permissions.GetAllPermissions();  // أو استعلام من الـ API
    private Modal modal;
    public void OpenModal()
    {
        modal.Show();
    }
    public void CloseModal()
    {
        modal.Hide();
    }

    private async Task AddRole()
    {
        var result = await RoleService.AddRoleAysnc(newRole);
        if (result.IsSuccess)
        {
         //   Navigation.NavigateTo("/admin/roles");
            await OnRoleCreated.InvokeAsync();
            CloseModal();
        }
        else
        {
            // Handle error
        }
    }

    //private void ShowSuccessToast()
    //{
    //    ToastService.ShowToast("This is a success message!", ToastType.Success);
    //}

    //private void ShowDangerToast()
    //{
    //    ToastService.ShowToast("This is a danger message!", ToastType.Danger);
    //}

}
