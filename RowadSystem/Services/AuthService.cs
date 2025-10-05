
using RowadSystem.Authentication;
using RowadSystem.Shard.consts;
using System.Security.Cryptography;

namespace RowadSystem.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider,
    ApplicationDbContext context,
    ISendOTPService sendOTPService) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ApplicationDbContext _context = context;
    private readonly ISendOTPService _sendOTPService = sendOTPService;
    private readonly int _refreshTokenExpiryDays = 15;

    public async Task<Result<AuthResponse>> LogInAsync(LoginRequest request)
    {

        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        if (!user.EmailConfirmed)
            return Result.Failure<AuthResponse>(UserErrors.EmailNotConfirmed);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

        if (result.Succeeded)
        {

            var (roles, permissions) = await GetUserRolesAndPermissionsAsync(user);

            var (token, expiration) = _jwtProvider.GenerateToken(user, roles, permissions);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddMinutes(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                Expiration = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            var authResponse = new AuthResponse
            (
                 user.Id,
                 user.Email!,
                 user.FirstName,
                user.LastName!,
                token,
                  expiration,
                  refreshToken,
                  refreshTokenExpiration
            );

            return Result.Success(authResponse);
        }

        return result.IsLockedOut ? Result.Failure<AuthResponse>(UserErrors.AccountLocked) : Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
    }
    public async Task<Result> RegisterAsync(RegisterRequest request)
    {

        if (await _userManager.Users.AnyAsync(x => x.Email == request.Email))
            return Result.Failure(UserErrors.UserAlreadyExists);

        var user = request.Adapt<ApplicationUser>();
        //user.Address = request.Address.Adapt<IAddress>();
        //if (user.Address is null)
        //    return Result.Failure(CustomerErrors.InvalidAddress);

        var phones = request.PhoneNumbers.Adapt<List<ContactNumber>>();

        if (phones is null || !phones.Any())
            return Result.Failure(CustomerErrors.InvalidPhoneNumbers);

        foreach (var phone in phones)
            user.ContactNumbers!.Add(phone);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _sendOTPService.SendOtpAsync(user.Email!, TemplatesName.OtpConfirmEmailTemplate);
        }

        return Result.Success();
    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var result = await _sendOTPService.VerifyOtpAsync(request.Email, request.Code);
        if (!result.IsSuccess)
            return Result.Failure(UserErrors.OtpFailed);

        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        user.EmailConfirmed = true;

        await _userManager.AddToRoleAsync(user, DefaultRole.Member);
        await _userManager.UpdateAsync(user);

        return Result.Success();

    }
    public async Task<Result> ForgetPasswordAsync(ForgetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        await _sendOTPService.SendOtpAsync(user.Email!, TemplatesName.OtpForgetPasswordTemplate);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var isVerify = await _sendOTPService.VerifyOtpAsync(request.Email, request.Code);
        if (!isVerify.IsSuccess)
            return Result.Failure(UserErrors.OtpFailed);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

        if (result.Succeeded)
            return Result.Success();

        return Result.Failure(UserErrors.InvalidCredentials);
    }


    private string GenerateRefreshToken()
    {
        var token = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(token);
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var userId = _jwtProvider.ValidateToken(request.token);
        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        var existingRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == request.refreshToken);

        if (existingRefreshToken is null || !existingRefreshToken.IsActive || existingRefreshToken.IsRevoked)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        var (roles, permissions) = await GetUserRolesAndPermissionsAsync(user);

        var (newToken, expiration) = _jwtProvider.GenerateToken(user, roles, permissions);
        var newRefreshToken = GenerateRefreshToken();
        var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        existingRefreshToken.RevokedAt = DateTime.UtcNow;

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            Expiration = newRefreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);

        var authResponse = new AuthResponse
        (
             user.Id,
             user.Email!,
             user.FirstName,
            user.LastName!,
            newToken,
              expiration,
              newRefreshToken,
              newRefreshTokenExpiration
        );
        return Result.Success(authResponse);
    }
    public async Task<Result> RevokedRefreshTokenAsync(RefreshTokenRequest request)
    {
        var userId = _jwtProvider.ValidateToken(request.token);
        if (userId is null)
            return Result.Failure(UserErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);


        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == request.refreshToken && x.IsActive);
        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);


        userRefreshToken.RevokedAt = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
    private async Task<(IList<string> Roles, IList<string> Permissions)> GetUserRolesAndPermissionsAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var permissions = await (from r in _context.Roles
                                 join p in _context.RoleClaims
                                 on r.Id equals p.RoleId
                                 where roles.Contains(r.Name!)
                                 select p.ClaimValue
                                 ).ToListAsync();

        return (roles, permissions);
    }
}
