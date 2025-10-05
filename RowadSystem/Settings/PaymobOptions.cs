namespace RowadSystem.API.Settings;

public class PaymobOptions
{
 
    public string ApiKey { get; set; } = "";
    public string IframeId { get; set; } = "";
    public int IntegrationIdCard { get; set; }
    public int IntegrationIdWallet { get; set; }
    public string HmacSecret { get; set; } = "";
    public string BaseUrl { get; set; } = "https://accept.paymob.com";
}
