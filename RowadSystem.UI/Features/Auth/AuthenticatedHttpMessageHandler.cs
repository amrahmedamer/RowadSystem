//using System.Net.Http.Headers;

//namespace RowadSystem.UI.Features.Auth;

//public class AuthenticatedHttpMessageHandler(ITokenStorageService tokenStorage) : DelegatingHandler
//{
//    private readonly ITokenStorageService _tokenStorage = tokenStorage;

//    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {
//        var token = await _tokenStorage.GetAccessTokenAsync();
//        if (!string.IsNullOrWhiteSpace(token))
//        {
//            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//        }

//        return await base.SendAsync(request, cancellationToken);
//    }
//}

using RowadSystem.Shard.Contract.Auth;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Auth;

public class AuthenticatedHttpMessageHandler(ITokenStorageService tokenStorage, IHttpClientFactory httpClientFactory) : DelegatingHandler
{
    private readonly ITokenStorageService _tokenStorage = tokenStorage;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("NoAuth");

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenStorage.GetAccessTokenAsync();
        var expiry = await _tokenStorage.GetExpiryAsync();


        if (expiry <= DateTime.Now)
        {
            var refreshToken = await _tokenStorage.GetRefreshTokenAsync();
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var refreshRequest = new RefreshTokenRequest
                (
                     accessToken,
                    refreshToken
                );

                var response = await _httpClient.PostAsJsonAsync("Auth/refresh-token", refreshRequest, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: cancellationToken);
                    if (result is not null)
                    {
                        await _tokenStorage.SaveTokenAsync(result.Token, result.RefreshToken, result.Expiration);
                        accessToken = result.Token;
                    }
                }
                else
                {
                    await _tokenStorage.RemoveTokensAsync();
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(accessToken) && !request.Headers.Contains("Authorization"))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
