using RowadSystem.API.Helpers;
using RowadSystem.Shard.consts;
using RowadSystem.Shard.Contract.Acounts;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Users;

namespace RowadSystem.Services;

public class UserService(UserManager<ApplicationUser> userManager,
    IRoleService roleServices,
    ApplicationDbContext context) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IRoleService _roleServices = roleServices;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<UserPorfileResponse>> GetUserProfileAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserPorfileResponse>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(user);

        return Result.Success((user, roles).Adapt<UserPorfileResponse>());
    }
    public async Task<Result> UpdateUserProfileAsync(string id, UpdateUserProfileRequest request)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserPorfileResponse>(UserErrors.UserNotFound);

        var result = await _userManager.UpdateAsync(request.Adapt(user));

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        return Result.Success();
    }
    public async Task<Result> ChangePasswordAsync(string id, ChangePasswordRequest request)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserPorfileResponse>(UserErrors.UserNotFound);

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        return Result.Success();
    }



    public async Task<Result<PaginatedListResponse<UserResponse>>> GetAllAsync(RequestFilters requestFilters)
    {
        //var users =  (
        //    from u in _context.Users
        //    join ur in _context.UserRoles
        //    on u.Id equals ur.UserId
        //    join r in _context.Roles
        //    on ur.RoleId equals r.Id into roles
        //    where !roles.Any(x => x.Name == DefaultRole.Member)
        //    select new UserResponse
        //    (
        //        u.Id,
        //        u.Email!,
        //        u.FirstName,
        //        u.LastName!,
        //        roles.Select(role => role.Name).ToList()!
        //    ));

        var users = (
            from u in _context.Users
            join ur in _context.UserRoles
            on u.Id equals ur.UserId
            join r in _context.Roles
            on ur.RoleId equals r.Id into roles
            where !roles.Any(x => x.Name == DefaultRole.Member)
            group new { u, roles } by new { u.Id, u.Email, u.FirstName, u.LastName } into userGroup
            select new UserResponse
            {
                Id = userGroup.Key.Id,
                Email = userGroup.Key.Email!,
                FirstName = userGroup.Key.FirstName,
                LastName = userGroup.Key.LastName!,
                Roles = userGroup.SelectMany(g => g.roles.Select(role => role.Name)).Distinct().ToList(),
                PhoneNumbers = userGroup.SelectMany(g => g.u.ContactNumbers).Select(p => p.Number).ToList()
            });


        var response = await PaginatedList<UserResponse>.CreatePaginationAsync(users, requestFilters.PageNumber, requestFilters.PageSize);
        if (response is null)
            return Result.Failure<PaginatedListResponse<UserResponse>>(UserErrors.UserNotFound);
       

        return Result.Success(response.Adapt<PaginatedListResponse<UserResponse>>());
    }
    public async Task<Result<UserResponse>> GetByIdAsync(string Id)
    {
        if (await _userManager.FindByIdAsync(Id) is not { } users)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(users);

        return Result.Success((users, roles).Adapt<UserResponse>());
    }

    public async Task<Result<UserResponse>> AddUserAsync(UserRequest request)
    {
        if (await _userManager.Users.AnyAsync(u => u.Email == request.Email))
            return Result.Failure<UserResponse>(UserErrors.UserAlreadyExists);

        var allowedRoles = await _roleServices.GetRolesAsync();

        if (request.Roles.Except(allowedRoles.Value).Any())
            return Result.Failure<UserResponse>(RoleErrors.RoleNotFound);

        var user = request.Adapt<ApplicationUser>();

        user.Address = request.Address.Adapt<Address>();
        if (user.Address is null)
            return Result.Failure<UserResponse>(CustomerErrors.InvalidAddress);

        var phones = request.PhoneNumbers.Adapt<List<ContactNumber>>();

        if (phones is null || !phones.Any())
            return Result.Failure<UserResponse>(CustomerErrors.InvalidPhoneNumbers);

        foreach (var phone in phones)
            user.ContactNumbers!.Add(phone);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var role = request.Roles.Select(x => x.Name).ToList();
            await _userManager.AddToRolesAsync(user, role);

            return Result.Success((user, request.Roles).Adapt<UserResponse>());
        }

        var errors = result.Errors.First();
        return Result.Failure<UserResponse>(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));

    }

    public async Task<Result> UpdateUserAsync(string id, UserRequest request)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        var updateUser = request.Adapt(user);
        var result = await _userManager.UpdateAsync(updateUser);

        if (result.Succeeded)
        {
            await _context.UserRoles
           .Where(x => x.UserId == id)
           .ExecuteDeleteAsync();
            var role = request.Roles.Select(x => x.Name).ToList();
            await _userManager.AddToRolesAsync(user, role);

            return Result.Success((user, request.Roles).Adapt<UserResponse>());
        }

        var errors = result.Errors.First();
        return Result.Failure<UserResponse>(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));

    }

    public async Task<Result> UnLookUser(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var lookedReseult = await _userManager.SetLockoutEndDateAsync(user, null);
        var resetResult = await _userManager.ResetAccessFailedCountAsync(user);

        if (!lookedReseult.Succeeded)
        {
            var error = lookedReseult.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        if (!resetResult.Succeeded)
        {
            var error = resetResult.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        return Result.Success();

    }

    public async Task<Result> ToggleStatus(string id)
    {

        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        user.IsDeleted = !user.IsDeleted;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }

}

