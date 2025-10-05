using RowadSystem.Shard.Contract.Paymob;

namespace RowadSystem.Services;

public interface IPaymobService
{
    Task<StartPaymentResponse> StartCardPaymentAsync(StartPaymentRequest request, CancellationToken ct);
    Task<StartPaymentResponse> StartWalletPaymentAsync(StartWalletPaymentRequest request, CancellationToken ct);
    bool VerifyHmac(PaymobWebhookPayload payload);
}
