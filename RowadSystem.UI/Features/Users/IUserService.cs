using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Roles;
using RowadSystem.Shard.Contract.Users;

namespace RowadSystem.UI.Features.Users;

public interface IUserService
{
    Task<PaginatedListResponse<UserResponse>> GetAllUsers(RequestFilters filters);
    Task<Result<HttpResponseMessage>> AddUser(UserRequest request);
    Task<List<string>> GetAllRoles();
}
