using Microsoft.Extensions.Options;
using RowadSystem.API.Settings;
using RowadSystem.Shard.Contract.Paymob;
using System.Security.Cryptography;
using System.Text;




public sealed class PaymobService : IPaymobService
{
    private readonly HttpClient _http;
    private readonly PaymobOptions _opt;

    public PaymobService(HttpClient http, IOptions<PaymobOptions> opt)
    {
        _http = http;
        _opt = opt.Value;
        _http.BaseAddress = new Uri(_opt.BaseUrl);
    }

    public async Task<StartPaymentResponse> StartCardPaymentAsync(StartPaymentRequest request, CancellationToken ct)
    {
       

        var auth = await _http.PostAsJsonAsync("/api/auth/tokens", new { api_key = _opt.ApiKey }, ct)
            .ResultEnsure().Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: ct);

        var order = await _http.PostAsJsonAsync("/api/ecommerce/orders", new
        {
            auth_token = auth!.token,
            delivery_needed = false,
            amount_cents = request.AmountCents,
            currency = request.Currency,
            items = Array.Empty<object>(),
            merchant_order_id = request.MerchantOrderId
        }, ct).ResultEnsure().Content.ReadFromJsonAsync<CreateOrderResponse>(cancellationToken: ct);

        var billing = ToBilling(request.Billing);

        var payKey = await _http.PostAsJsonAsync("/api/acceptance/payment_keys", new
        {
            auth_token = auth!.token,
            amount_cents = request.AmountCents,
            expiration = 3600,
            order_id = order!.id,
            billing_data = billing,
            currency = request.Currency,
            integration_id = _opt.IntegrationIdCard
        }, ct).ResultEnsure().Content.ReadFromJsonAsync<PaymentKeyResponse>(cancellationToken: ct);

        return new StartPaymentResponse
        {
            OrderId = order!.id,
            IframeUrl = $"{_opt.BaseUrl}/api/acceptance/iframes/{_opt.IframeId}?payment_token={payKey!.token}"
        };
    }

    public async Task<StartPaymentResponse> StartWalletPaymentAsync(StartWalletPaymentRequest request, CancellationToken ct)
    {
        var auth = await _http.PostAsJsonAsync("/api/auth/tokens", new { api_key = _opt.ApiKey }, ct)
            .ResultEnsure().Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: ct);

        var order = await _http.PostAsJsonAsync("/api/ecommerce/orders", new
        {
            auth_token = auth!.token,
            delivery_needed = false,
            amount_cents = request.AmountCents,
            currency = request.Currency,
            items = Array.Empty<object>(),
            merchant_order_id = request.MerchantOrderId
        }, ct).ResultEnsure().Content.ReadFromJsonAsync<CreateOrderResponse>(cancellationToken: ct);

        var billing = ToBilling(request.Billing);

        var payKey = await _http.PostAsJsonAsync("/api/acceptance/payment_keys", new
        {
            auth_token = auth!.token,
            amount_cents = request.AmountCents,
            expiration = 3600,
            order_id = order!.id,
            billing_data = billing,
            currency = request.Currency,
            integration_id = _opt.IntegrationIdWallet
        }, ct).ResultEnsure().Content.ReadFromJsonAsync<PaymentKeyResponse>(cancellationToken: ct);

        var walletPay = await _http.PostAsJsonAsync("/api/acceptance/payments/pay", new
        {
            source = new { identifier = request.WalletMsisdn, subtype = "WALLET" },
            payment_token = payKey!.token
        }, ct).ResultEnsure().Content.ReadFromJsonAsync<WalletPayResponse>(cancellationToken: ct);

        return new StartPaymentResponse
        {
            OrderId = order!.id,
            RedirectUrl = walletPay!.redirect_url
        };
    }

    public bool VerifyHmac(PaymobWebhookPayload p)
    {
        // Use official order from Paymob docs for transaction callback:
        // Update the exact fields if your payload differs.
        string[] fieldsOrder =
        {
            "amount_cents","created_at","currency","error_occured","has_parent_transaction","id",
            "integration_id","is_3d_secure","is_auth","is_capture","is_refunded","is_standalone_payment",
            "is_voided","order.id","owner","pending","source_data.pan","source_data.sub_type","source_data.type","success"
        };

        string GetValue(string path)
        {
            return path switch
            {
                "amount_cents" => p.amount_cents ?? "",
                "currency" => p.currency ?? "",
                "id" => p.id.ToString(),
                "integration_id" => p.integration_id.ToString(),
                "order.id" => p.order?.id.ToString() ?? "",
                "pending" => p.pending.ToString().ToLowerInvariant(),
                "source_data.pan" => p.source_data?.pan ?? "",
                "source_data.sub_type" => p.source_data?.sub_type ?? "",
                "source_data.type" => p.source_data?.type ?? "",
                "success" => p.success.ToString().ToLowerInvariant(),
                // Optional/nullable fields; set empty if not present
                "created_at" => "",
                "error_occured" => "false",
                "has_parent_transaction" => "false",
                "is_3d_secure" => "false",
                "is_auth" => "false",
                "is_capture" => "false",
                "is_refunded" => "false",
                "is_standalone_payment" => "true",
                "is_voided" => "false",
                "owner" => "",
                _ => ""
            };
        }

        var concatenated = new StringBuilder();
        foreach (var f in fieldsOrder)
            concatenated.Append(GetValue(f));

        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_opt.HmacSecret));
        var hash = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(concatenated.ToString()))).ToLowerInvariant();
        return hash == (p.hmac ?? "").ToLowerInvariant();
    }

    private static object ToBilling(BillingData b) => new
    {
        apartment = "NA",
        email = b.Email,
        floor = "NA",
        first_name = b.FirstName,
        street = "NA",
        building = "NA",
        phone_number = b.Phone,
        shipping_method = "NA",
        postal_code = "NA",
        city = b.City,
        country = b.Country,
        last_name = b.LastName,
        state = "C"
    };

    private sealed class AuthResponse { public string token { get; set; } = ""; }
    private sealed class CreateOrderResponse { public long id { get; set; } }
    private sealed class PaymentKeyResponse { public string token { get; set; } = ""; }
    private sealed class WalletPayResponse { public string redirect_url { get; set; } = ""; }
}

internal static class HttpResponseMessageExtensions
{
    public static HttpResponseMessage ResultEnsure(this Task<HttpResponseMessage> task)
    {
        var resp = task.GetAwaiter().GetResult();
        resp.EnsureSuccessStatusCode();
        return resp;
    }
}