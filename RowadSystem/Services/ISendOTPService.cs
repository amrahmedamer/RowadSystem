
namespace RowadSystem.Services;

public interface ISendOTPService
{
    Task<Result> SendOtpAsync(string email, string templateName);
    Task<Result> VerifyOtpAsync(string email, string code);

}
