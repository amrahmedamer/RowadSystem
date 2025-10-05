namespace RowadSystem.Settings;

public class EmailSettings
{
    public int Port { get; set; }
    public string Host { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
}
