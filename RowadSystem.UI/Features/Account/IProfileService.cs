using RowadSystem.Shard.Contract.Acounts;

namespace RowadSystem.UI.Features.Account;

public interface IProfileService
{
    Task<UserPorfileResponse> GetUserProfileAsync();
}
