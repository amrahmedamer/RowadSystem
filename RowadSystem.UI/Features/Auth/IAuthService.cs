using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Auth;

namespace RowadSystem.UI.Features.Auth;

public interface IAuthService
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest);
    Task RegisterAsync(RegisterRequest registerRequest);
    Task ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
    Task ResendOtpAsync(OtpRequest otpRequest);
 
}
