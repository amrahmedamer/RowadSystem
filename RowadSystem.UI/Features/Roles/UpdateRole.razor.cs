//using Microsoft.AspNetCore.Components;
//using RowadSystem.Shard.consts;
//using RowadSystem.Shard.Contract.Roles;
//using System.Threading.Tasks;

//namespace RowadSystem.UI.Features.Roles;

//public partial class UpdateRole
//{
//    [Inject]
//    public IRoleService RoleService { get; set; } = default!;
//    [Inject]
//    public NavigationManager Navigation { get; set; } = default!;
//    public string RoleId { get; set; }= "415c57e6-ab9b-4fea-b865-57190cd196f3";
//    private RoleRequest roleToEdit = new RoleRequest();
//    private Modal modal;
//    public async Task OpenModalAsync()
//    {
//        await LoadRole();
//        modal.Show();
//    }
//    public void CloseModal()
//    {
//        modal.Hide();


//    }

//    private async Task EditRole()
//    {

//        Console.WriteLine($" EditRole");
//        Console.WriteLine($"RoleId : {RoleId} , roleToEdit : {roleToEdit.Role}");
//        var result = await RoleService.UpdateRoleAysnc(RoleId, roleToEdit);
//        if (result.IsSuccess)
//        {
//            Navigation.NavigateTo("/admin/roles");
//        }
//        else
//        {
//            // Handle error
//        }
//    }
//   public async Task LoadRole()
//    {
//        var roleResult = await RoleService.GetRoleByIdAsync(RoleId);
//        if (roleResult.IsSuccess)
//        {
//            roleToEdit = new RoleRequest
//            {
//                Role = roleResult.Value.Name
//            };
//        }
//        StateHasChanged();
//    }


//}


using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Roles;

namespace RowadSystem.UI.Features.Roles
{
    public partial class UpdateRole
    {
        [Inject]
        public IRoleService RoleService { get; set; } = default!;
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;
        [Parameter] public EventCallback OnRoleUpdated { get; set; }

        public string RoleId { get; set; } = string.Empty;
        private RoleRequest roleToEdit = new RoleRequest();
        private Modal modal;
        private bool isLoading = false;
        private string errorMessage = string.Empty;
        private Roles _rolesComponent;
        public async Task OpenModalAsync()
        {
            await LoadRoles();
            modal.Show();
            //isLoading = true;  
            //isLoading = false;  

        }

        public void CloseModal()
        {
            modal.Hide();
        }

        private async Task EditRole()
        {

            //isLoading = true;
            var result = await RoleService.UpdateRoleAysnc(RoleId, roleToEdit);

            if (result.IsSuccess)
            {
                await OnRoleUpdated.InvokeAsync();
                CloseModal();
            }
            else
            {
                errorMessage = "Failed to update role. Please try again.";
            }

            //isLoading = false;

        }

        // Load the role information
        public async Task LoadRoles()
        {

            var roleResult = await RoleService.GetRoleByIdAsync(RoleId);
            if (roleResult.IsSuccess)
            {
                roleToEdit.Role = roleResult.Value.Name;
            }
            else
            {
                errorMessage = "Role not found.";
            }

            //StateHasChanged(); // Refresh the UI
        }
       
    }
}
