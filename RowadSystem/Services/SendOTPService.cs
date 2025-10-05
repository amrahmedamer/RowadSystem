using System.Security.Cryptography;
using System.Text;

namespace RowadSystem.Services;

public class SendOTPService(IEmailService emailService,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context
    ) : ISendOTPService
{
    private readonly IEmailService _emailService = emailService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> SendOtpAsync(string email, string templateName)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        var existingOtp = await _context.OTP
            .Where(otp => otp.UserId == user.Id && !otp.IsUsed && otp.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();

        if (existingOtp != null)
            existingOtp.IsUsed = true;

        var otp = new Random().Next(100000, 999999);

        var code = HashSHA256(otp.ToString());

        var otpEntity = new OTP
        {
            UserId = user.Id,
            code = code
        };

        await _context.OTP.AddAsync(otpEntity);
        await _context.SaveChangesAsync();

        await _emailService.SendEmailAsync(user.Email!,
               "Your OTP Code",
               templateName,
               new Dictionary<string, string>
               {
                    { "{{FirstName}}", user.FirstName },
                    { "{{OTP_CODE}}", otp.ToString() },
                    { "{{ExpiryMinutes}}", "1" },
               });

        return Result.Success(code);
    }

    public async Task<Result> VerifyOtpAsync(string email, string code)
    {

        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        var hashCode = HashSHA256(code);

        var otp = await _context.OTP.FirstOrDefaultAsync(otp =>
                                        otp.code == hashCode
                                        && !otp.IsUsed
                                        && otp.ExpiresAt > DateTime.UtcNow
                                        && otp.UserId == user.Id
                                    );


        if (otp is null)
            return Result.Failure(UserErrors.OtpFailed);

        otp.IsUsed = true;
        otp.ExpiresAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    private string HashSHA256(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

}
