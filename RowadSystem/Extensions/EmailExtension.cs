namespace RowadSystem.Extensions;

public static class EmailExtension
{
    public static async Task SendEmailAsync(this IEmailService emailService, string email, string subject, string templateName, Dictionary<string, string> replace)
    {

        var body = File.ReadAllText($"Templates/{templateName}.html");

        foreach (var item in replace)
            body = body.Replace(item.Key, item.Value);

        await emailService.SendEmailAsync(email, subject, body);

    }
}
