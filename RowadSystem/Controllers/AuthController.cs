using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.consts;

namespace RowadSystem.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService,
    ISendOTPService sendOTPService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ISendOTPService _sendOTPService = sendOTPService;

    [HttpPost("login")]
    public async Task<IActionResult> LogIn(LoginRequest request)
    {
        var result = await _authService.LogInAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }

    [HttpPost("register")]
    public async Task<IActionResult> register(RegisterRequest request)
    {

        var result = await _authService.RegisterAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();

    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {

        var result = await _authService.ConfirmEmailAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();

    }
    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp([FromBody] OtpRequest request)
    {
        var result = await _sendOTPService.SendOtpAsync(request.Email, TemplatesName.ResendOtpTemplate);
        return result.IsSuccess ? Ok() : result.ToProblem();

    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.ForgetPasswordAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {

        var result = await _authService.ResetPasswordAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();

    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }
    [HttpPost("revoked-refresh-token")]
    public async Task<IActionResult> RevokedRefreshToken(RefreshTokenRequest request)
    {
        var result = await _authService.RevokedRefreshTokenAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();

    }
}
