namespace RowadSystem.UI.Features.Auth;

public interface ITokenStorageService
{
    Task SaveTokenAsync(string accessToken, string refreshToken, int expirationSeconds);
    Task<string?> GetAccessTokenAsync();
    Task<string?> GetRefreshTokenAsync();
    Task RemoveTokensAsync();
    Task<DateTime?> GetExpiryAsync();
}
