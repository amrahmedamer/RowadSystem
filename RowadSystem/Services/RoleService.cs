using Azure;
using Azure.Core;
using CloudinaryDotNet.Actions;
using RowadSystem.Shard.consts;
using RowadSystem.Shard.Contract.Roles;
using Error = RowadSystem.Shard.Abstractions.Error;

namespace RowadSystem.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

    //public async Task<Result<List<string?>>> GetRolesAsync(bool includeDisabled = false)
    //  => Result.Success(await _roleManager.Roles
    //      .Where(x => !x.IsDefault && (x.IsActive || includeDisabled == true))
    //      .Select(r => r.Name)
    //      .ToListAsync());
    public async Task<Result<List<RoleRespons>>> GetRolesAsync(bool includeDisabled = false)
    {
        var response = await _roleManager.Roles
           .Where(x => !x.IsDefault && (x.IsActive || includeDisabled == true))
           .Select(r => new RoleRespons
           {
               Id = r.Id,
               IsActive = r.IsActive,
               Name = r.Name,
           })
           .ToListAsync();

        return Result.Success(response);
    }
    public async Task<Result<List<string>>> GetAllRolesAsync()
    {
        var response = await _roleManager.Roles
           .Where(x => !x.IsDefault && x.IsActive)
           .Select(r => r.Name)
           .ToListAsync();

        if(response is null)
            return Result.Failure<List<string>>(RoleErrors.RoleNotFound);

        return Result.Success(response);
    }



    public async Task<Result<RoleRespons>> GetRoleByIdAsync(string Id)
    {

        if (await _roleManager.FindByIdAsync(Id) is not { } role)
            return Result.Failure<RoleRespons>(RoleErrors.RoleNotFound);

        var permissions = await _roleManager.GetClaimsAsync(role);

        var response = new RoleRespons
        {
            Id = role.Id,
            Name = role.Name!,
            IsActive = role.IsActive,
            Permissions = permissions.Select(x => x.Value)

        };

        return Result.Success(response);
    }

    public async Task<Result> AddRoleAysnc(RoleRequest request)
    {
        if (await _roleManager.RoleExistsAsync(request.Role))
            return Result.Failure<RoleRespons>(RoleErrors.RoleAlreadyExists);

        //var permissions = Permissions.GetAllPermissions();

        //if (request.Permissions.Except(permissions).Any())
        //    return Result.Failure<RoleRespons>(RoleErrors.PermissionNotFound);

        var role = new ApplicationRole
        {
            Name = request.Role,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            //var newPermissions = request.Permissions
            //    .Select(p => new IdentityRoleClaim<string>
            //    {
            //        RoleId = role.Id,
            //        ClaimType = Permissions.Type,
            //        ClaimValue = p
            //    })
            //    .ToList();

            //await _context.AddRangeAsync(newPermissions);
            //await _context.SaveChangesAsync();

            //var response = new RoleRespons
            //{
            //    Id = role.Id,
            //    Name = role.Name!,
            //    IsActive = role.IsActive
            //    //Permissions = request.Permissions
            //};

            return Result.Success();

        }
        var errors = result.Errors.First();
        return Result.Failure<RoleRespons>(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> UpdateRolePermissionAysnc(string RoleId, PermissionRequest request)
    {


        if (await _roleManager.FindByIdAsync(RoleId) is not { } role)
            return Result.Failure<RoleRespons>(RoleErrors.RoleNotFound);

        var permissions = Permissions.GetAllPermissions();

        if (request.Permissions.Except(permissions).Any())
            return Result.Failure<RoleRespons>(RoleErrors.PermissionNotFound);

        var currentPermissions = await _context.RoleClaims
            .Where(rc => rc.RoleId == role.Id && rc.ClaimType == Permissions.Type)
            .Select(rc => rc.ClaimValue)
            .ToListAsync();

        var newPermissions = request.Permissions.Except(currentPermissions)
            .Select(p => new IdentityRoleClaim<string>
            {
                RoleId = role.Id,
                ClaimType = Permissions.Type,
                ClaimValue = p
            });

        var removedPermissions = currentPermissions.Except(request.Permissions).ToList();

        if (removedPermissions.Any())
        {
            var claimsToRemove = await _context.RoleClaims
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == Permissions.Type && removedPermissions.Contains(rc.ClaimValue))
                .ExecuteDeleteAsync();

        }

        if (newPermissions.Any())
            await _context.RoleClaims.AddRangeAsync(newPermissions);

        await _context.SaveChangesAsync();


        //var response = new RoleRespons
        //{
        //    Id = role.Id,
        //    Name = role.Name!,
        //    IsActive = role.IsActive,
        //    Permissions = request.Permissions
        //};

        return Result.Success();


    }
    public async Task<Result> UpdateRoleAysnc(string id, RoleRequest request)
    {
        var roleExists = await _roleManager.Roles.AnyAsync(x => x.Name == request.Role && x.Id != id);
        if (roleExists)
            return Result.Failure<RoleRespons>(RoleErrors.RoleAlreadyExists);

        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleRespons>(RoleErrors.RoleNotFound);

        role.Name = request.Role;
        var result = await _roleManager.UpdateAsync(role);


        if (result.Succeeded)
            return Result.Success();

    
        var errors = result.Errors.First();
        return Result.Failure<RoleRespons>(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> ToggleStatusAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
            return Result.Failure(RoleErrors.RoleNotFound);

        role.IsActive = !role.IsActive;
        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
            return Result.Success();

        var errors = result.Errors.First();
        return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
    }
}
