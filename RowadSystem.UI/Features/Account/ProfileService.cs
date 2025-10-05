using Microsoft.AspNetCore.Components.Authorization;
using RowadSystem.Shard.Contract.Acounts;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Account;

public class ProfileService(IHttpClientFactory httpClientFactory): IProfileService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AuthorizedClient");
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public async Task<UserPorfileResponse> GetUserProfileAsync()
    {
        //var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        //var token = state.User?.FindFirst(c => c.Type == "access_token")?.Value;

        //if (string.IsNullOrEmpty(token)) return null;

        var response = await _httpClient.GetAsync("/me");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserPorfileResponse>();
        }

        return null;
    }

}
