namespace RowadSystem.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> LogInAsync(LoginRequest request);
    Task<Result> RegisterAsync(RegisterRequest request);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ForgetPasswordAsync(ForgetPasswordRequest request);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
    Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<Result> RevokedRefreshTokenAsync(RefreshTokenRequest request);
}
