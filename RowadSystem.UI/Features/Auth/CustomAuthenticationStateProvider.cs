using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace RowadSystem.UI.Features.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("access_token");
        var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    //public void MarkUserAsAuthenticated(string token)
    //{
    //    var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
    //    var user = new ClaimsPrincipal(identity);
    //    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    //}
    public async Task MarkUserAsAuthenticated(string token)
    {
        await _localStorage.SetItemAsync("access_token", token);
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOutAsync()
    {
        // Remove token from LocalStorage
        await _localStorage.RemoveItemAsync("access_token");
        // Clear Authentication State
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = WebEncoders.Base64UrlDecode(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    // Check if the token has expired and handle accordingly
    public async Task<bool> IsTokenExpired()
    {
        var token = await _localStorage.GetItemAsync<string>("access_token");
        if (string.IsNullOrEmpty(token)) return true;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        if (jwtToken == null) return true;

        var expiryDateUnix = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
        if (expiryDateUnix == null) return true;

        var expiryDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiryDateUnix)).UtcDateTime;
        return expiryDate < DateTime.Now;
    }
}
