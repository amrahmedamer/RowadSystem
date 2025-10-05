
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Auth;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Auth;

public class AuthService(IHttpClientFactory httpClientFactory) : IAuthService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("NoAuth");

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("/Auth/login", loginRequest);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<AuthResponse>(response);
        }
    }


    public async Task RegisterAsync(RegisterRequest registerRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("/Auth/register", registerRequest);

        if (!response.IsSuccessStatusCode)
        {
            //return await PorblemDetailsExtensions.HandleErrorResponse(response);

        }
    }
    public async Task ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("/Auth/confirm-email", confirmEmailRequest);

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        }
    }
    public async Task ResendOtpAsync(OtpRequest otpRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("/Auth/resend-otp", otpRequest);

        if (!response.IsSuccessStatusCode)
        {
        }
    }
  

}
