using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Acounts;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Users;

namespace RowadSystem.Services;

public interface IUserService
{
    Task<Result<PaginatedListResponse<UserResponse>>> GetAllAsync(RequestFilters requestFilters);
    Task<Result<UserResponse>> GetByIdAsync(string Id);
    Task<Result<UserResponse>> AddUserAsync(UserRequest request);
    Task<Result> UpdateUserAsync(string id, UserRequest request);
    Task<Result> ToggleStatus(string id);
    Task<Result> UnLookUser(string id);
    Task<Result> UpdateUserProfileAsync(string id, UpdateUserProfileRequest request);
    Task<Result<UserPorfileResponse>> GetUserProfileAsync(string id);
    Task<Result> ChangePasswordAsync(string id, ChangePasswordRequest request);
}
