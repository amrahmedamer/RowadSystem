using Blazored.LocalStorage;

namespace RowadSystem.UI.Features.Auth;

public class TokenStorageService(ILocalStorageService localStorage) : ITokenStorageService
{
    private readonly ILocalStorageService _localStorage = localStorage;
    private const string AccessTokenKey = "access_token";
    private const string RefreshTokenKey = "refresh_token";
    private const string ExpirationSeconds = "token_expiry";

    public async Task SaveTokenAsync(string accessToken, string refreshToken, int expirationSeconds)
    {
        await _localStorage.SetItemAsync(AccessTokenKey, accessToken);
        await _localStorage.SetItemAsync(RefreshTokenKey, refreshToken);
        await _localStorage.SetItemAsync(ExpirationSeconds, expirationSeconds);
    }

    public async Task<DateTime?> GetExpiryAsync()
    {
        var expireAt = await _localStorage.GetItemAsync<int>(ExpirationSeconds);
        return expireAt > 0 ? DateTime.Now.AddSeconds(expireAt) : null;
    }
    public async Task<string?> GetAccessTokenAsync()
       => await _localStorage.GetItemAsync<string>(AccessTokenKey);

    public async Task<string?> GetRefreshTokenAsync()
        => await _localStorage.GetItemAsync<string>(RefreshTokenKey);

    public async Task RemoveTokensAsync()
    {
        await _localStorage.RemoveItemAsync(AccessTokenKey);
        await _localStorage.RemoveItemAsync(RefreshTokenKey);
    }
}
