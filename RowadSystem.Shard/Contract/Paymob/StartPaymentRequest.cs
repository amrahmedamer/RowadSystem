

namespace RowadSystem.Shard.Contract.Paymob;
public class StartPaymentRequest
{
    public int? AmountCents { get; set; }
    public string? Currency { get; set; } = "EGP";
    public string? MerchantOrderId { get; set; } = Guid.NewGuid().ToString("N");
    public BillingData? Billing { get; set; } = new();
}
