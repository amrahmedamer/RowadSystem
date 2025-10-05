namespace RowadSystem.Services;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string toEmail, string subject, string body);

}
