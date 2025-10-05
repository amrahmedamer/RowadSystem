using Microsoft.Extensions.Options;
using RowadSystem.Settings;
using System.Net;
using System.Net.Mail;

namespace RowadSystem.Services;

public class EmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly EmailSettings _emailSettings = options.Value;
    public async Task<Result> SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_emailSettings.UserName, _emailSettings.Sender),
            Subject = subject,
            IsBodyHtml = true
        };

        message.Body = body;
        message.To.Add(toEmail);


        try
        {

            using var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password)
            };

            await smtpClient.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("EmailService", "Failed to send email", StatusCodes.Status400BadRequest));
        }



        return Result.Success();
    }


}
