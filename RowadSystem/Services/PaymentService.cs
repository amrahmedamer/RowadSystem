using Microsoft.Extensions.Options;

using RowadSystem.API.Settings;
using RowadSystem.Shard.Contract.Paymob;


namespace RowadSystem.Services;

public class PaymentService(ApplicationDbContext context,  IOptions<PaymobOptions> options, HttpClient httpClient) : IPaymentService
{
    private readonly ApplicationDbContext _context = context;
    private readonly PaymobOptions _options = options.Value;
    //private readonly PaymobClient _paymobClient = paymobClient;
    private readonly HttpClient _httpClient = httpClient;
    //public async Task<PaymobOrderResponse> RegisterOrderAsync(OrderRegistrationRequest orderRequest, CancellationToken cancellationToken = default)
    //{
    //    if (orderRequest is null)
    //        throw new ArgumentNullException(nameof(orderRequest));

    //    // 1) Auth
    //    var auth = await _paymobClient.AuthenticateAsync(new AuthRequest
    //    {
    //        ApiKey = _options.ApiKey
    //    }, cancellationToken).ConfigureAwait(false);

    //    if (auth is null || string.IsNullOrWhiteSpace(auth.Token))
    //        throw new InvalidOperationException("Paymob authentication failed.");

    //    var orderRegistrationRequest = new OrderRegistrationRequest
    //    {
    //        AuthToken = auth.Token,
    //        AmountCents = orderRequest.AmountCents.ToString(),   
    //        Currency = string.IsNullOrWhiteSpace(orderRequest.Currency) ? "EGP" : orderRequest.Currency,
    //        DeliveryNeeded = "false",
    //        Items = orderRequest.Items?.Select(item => new OrderItem
    //        {
    //            Name = item.Name,
    //            Quantity = item.Quantity,
    //            AmountCents = item.AmountCents * 100
    //        }).ToList()!,
    //        ShippingData = new ShippingData
    //        {
    //            FirstName = orderRequest.ShippingData.FirstName ?? "NA",
    //            LastName = orderRequest.ShippingData.LastName,
    //            Email = orderRequest.ShippingData.Email,
    //            PhoneNumber = orderRequest.ShippingData.PhoneNumber ?? "EG",
    //        },
    //        ApiSource = orderRequest.ApiSource,             
    //        Integrations = orderRequest.Integrations ?? new List<int>() 
    //    };

    //    var orderRegistrationResponse = await _paymobClient.RegisterOrderAsync(orderRegistrationRequest).ConfigureAwait(false);
    //    if (orderRegistrationResponse is null || orderRegistrationResponse.Id <= 0)
    //        throw new InvalidOperationException("Failed to register order at Paymob.");

    //    // 3) Generate Payment Key
    //    var paymentKeyRequest = new PaymentKeyRequest
    //    {
    //        AuthToken = auth.Token,
    //        OrderId = orderRegistrationResponse.Id,
    //        Currency = string.IsNullOrWhiteSpace(orderRequest.Currency) ? "EGP" : orderRequest.Currency,
    //        IntegrationId = _options.IntegrationIdCard,        // استخدم IntegrationIdWallet للمحافظ
    //        Expiration = 3600,
    //        BillingData = new Paymob.Net.Models.BillingData
    //        {
    //            FirstName = "John",
    //            LastName = "Doe",
    //            Email = "user@example.com",
    //            PhoneNumber = "01000000000",
    //            City = "Cairo",
    //            Country = "EG",
    //            Apartment = "NA",
    //            Floor = "NA",
    //            Street = "NA",
    //            Building = "NA",
    //            ShippingMethod = "NA",
    //            PostalCode = "NA",
    //            State = "C"
    //        }
    //    };

    //    var paymentKeyResponse = await _paymobClient.RequestPaymentKeyAsync(paymentKeyRequest).ConfigureAwait(false);
    //    if (paymentKeyResponse is null || string.IsNullOrWhiteSpace(paymentKeyResponse.Token))
    //        throw new InvalidOperationException("Failed to generate payment key.");

    //    var iframeUrl = $"{_options.BaseUrl}/api/acceptance/iframes/{_options.IframeId}?payment_token={paymentKeyResponse.Token}";

    //    return new PaymobOrderResponse
    //    {
    //        OrderId = orderRegistrationResponse.Id.ToString(),
    //        PaymentKey = paymentKeyResponse.Token,
    //        IframeUrl = iframeUrl,
    //        Status = "pending"
    //    };
    //}
    //private sealed class AuthResponse {
    //    public string token { get; set; } }
    //public async Task<PaymobOrderResponse> RegisterOrderAsync(OrderRegistrationRequestDTO orderRequest, CancellationToken cancellationToken = default)
    //{
    //    if (orderRequest is null)
    //        throw new ArgumentNullException(nameof(orderRequest));
    //    //// 1) Auth
    //    //AuthResponse auth = null;
    //    //try
    //    //{
    //    //    auth = await _paymobClient.AuthenticateAsync(new AuthRequest
    //    //    {
    //    //        ApiKey = _options.ApiKey
    //    //    }).ConfigureAwait(false);

    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    // طباعة تفاصيل الاستثناء
    //    //    Console.WriteLine($"Error: {ex.Message}");
    //    //    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
    //    //}
    //    var authRequest = new AuthRequest { ApiKey = _options.ApiKey };
    //    var auth = await _paymobClient.AuthenticateAsync(authRequest, cancellationToken);


    //    // 1) Auth
    //    //var response = await _httpClient.PostAsJsonAsync($"{_options.BaseUrl}/api/auth/tokens", new { api_key = _options.ApiKey });

    //    //var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();

    //    if (auth is null || string.IsNullOrEmpty(auth.Token))
    //        throw new InvalidOperationException("Paymob authentication failed.");

    //    // 2) جهّز نفس orderRequest (بدون إنشاء نسخة جديدة)
    //    orderRequest.AuthToken = auth.Token;
    //    orderRequest.Currency = string.IsNullOrWhiteSpace(orderRequest.Currency) ? "EGP" : orderRequest.Currency;
    //    orderRequest.DeliveryNeeded = string.IsNullOrWhiteSpace(orderRequest.DeliveryNeeded) ? "false" : orderRequest.DeliveryNeeded;

    //    // Items: لو null خلّيها قائمة فاضية
    //    if (orderRequest.Items == null)
    //        orderRequest.Items = new List<OrderItemDTO>();
    //    else
    //    {
    //        // لو AmountCents في العناصر بالفعل بالقروش، لا تغيّره.
    //        // لو كان بالجنيه عندك، هنا فقط اضرب * 100.
    //        // المثال التالي يترك القيمة كما هي بافتراض أنها بالقروش:
    //        foreach (var it in orderRequest.Items)
    //        {
    //            if (it == null) continue;
    //            it.Name = string.IsNullOrWhiteSpace(it.Name) ? "Item" : it.Name;
    //            it.Quantity = it.Quantity <= 0 ? 1 : it.Quantity;
    //            it.AmountCents = it.AmountCents; // لا تعديل
    //        }
    //    }

    //    // ShippingData: امنع null وحدد افتراضيات
    //    orderRequest.ShippingData ??= new ShippingDataDTO();
    //    orderRequest.ShippingData.FirstName = string.IsNullOrWhiteSpace(orderRequest.ShippingData.FirstName) ? "John" : orderRequest.ShippingData.FirstName;
    //    orderRequest.ShippingData.LastName = string.IsNullOrWhiteSpace(orderRequest.ShippingData.LastName) ? "Doe" : orderRequest.ShippingData.LastName;
    //    orderRequest.ShippingData.Email = string.IsNullOrWhiteSpace(orderRequest.ShippingData.Email) ? "user@example.com" : orderRequest.ShippingData.Email;
    //    orderRequest.ShippingData.PhoneNumber = string.IsNullOrWhiteSpace(orderRequest.ShippingData.PhoneNumber) ? "01000000000" : orderRequest.ShippingData.PhoneNumber;

    //    // 3) Register Order
    //    var orderRegistrationResponse = await _paymobClient.RegisterOrderAsync(orderRequest.Adapt<OrderRegistrationRequest>()).ConfigureAwait(false);
    //    if (orderRegistrationResponse is null || orderRegistrationResponse.Id <= 0)
    //        throw new InvalidOperationException("Failed to register order at Paymob.");

    //    // 4) Payment Key
    //    // OrderRegistrationRequest.AmountCents عادة string -> حوّلها لـ int
    //    if (!int.TryParse(orderRequest.AmountCents, out var amountCentsInt))
    //        throw new InvalidOperationException("Invalid AmountCents format. Expected numeric string in cents.");

    //    var paymentKeyRequest = new PaymentKeyRequest
    //    {
    //        AuthToken = auth.Token,
    //        OrderId = orderRegistrationResponse.Id,
    //        AmountCents = amountCentsInt,
    //        Currency = orderRequest.Currency,
    //        IntegrationId = _options.IntegrationIdCard, // للمحفظة استخدم _options.IntegrationIdWallet
    //        Expiration = 3600,
    //        BillingData = new Paymob.Net.Models.BillingData
    //        {
    //            FirstName = orderRequest.ShippingData.FirstName,
    //            LastName = orderRequest.ShippingData.LastName,
    //            Email = orderRequest.ShippingData.Email,
    //            PhoneNumber = orderRequest.ShippingData.PhoneNumber,
    //            City = "Cairo",
    //            Country = "EG",
    //            Apartment = "NA",
    //            Floor = "NA",
    //            Street = "NA",
    //            Building = "NA",
    //            ShippingMethod = "NA",
    //            PostalCode = "NA",
    //            State = "C"
    //        }
    //    };

    //    var paymentKeyResponse = await _paymobClient.RequestPaymentKeyAsync(paymentKeyRequest).ConfigureAwait(false);
    //    if (paymentKeyResponse is null || string.IsNullOrWhiteSpace(paymentKeyResponse.Token))
    //        throw new InvalidOperationException("Failed to generate payment key.");

    //    var iframeUrl = $"{_options.BaseUrl}/api/acceptance/iframes/{_options.IframeId}?payment_token={paymentKeyResponse.Token}";

    //    return new PaymobOrderResponse
    //    {
    //        OrderId = orderRegistrationResponse.Id.ToString(),
    //        PaymentKey = paymentKeyResponse.Token,
    //        IframeUrl = iframeUrl,
    //        Status = "pending"
    //    };

    //}











    //public async Task<PaymobOrderResponse> RegisterOrderAsync(OrderRegistrationRequestDTO orderRequest)
    //{
    //    if (orderRequest is null)
    //        throw new ArgumentNullException(nameof(orderRequest));
    //    //// 1) التوثيق مع Paymob API باستخدام API Key
    //    //var authRequest = new AuthRequest { ApiKey = _options.ApiKey };
    //    //var auth = await _paymobClient.AuthenticateAsync(authRequest).ConfigureAwait(false);
    //    // 1) Auth
    //    var response = await _httpClient.PostAsJsonAsync($"{_options.BaseUrl}/api/auth/tokens", new { api_key = _options.ApiKey });

    //    var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();



    //    if (auth is null || string.IsNullOrEmpty(auth.Token))
    //        throw new InvalidOperationException("Paymob authentication failed.");

    //    // 2) تجهيز نفس orderRequest
    //    orderRequest.AuthToken = auth.Token;
    //    orderRequest.Currency = string.IsNullOrWhiteSpace(orderRequest.Currency) ? "EGP" : orderRequest.Currency;
    //    orderRequest.DeliveryNeeded = string.IsNullOrWhiteSpace(orderRequest.DeliveryNeeded) ? "false" : orderRequest.DeliveryNeeded;

    //    // Items: لو null خليها قائمة فاضية
    //    if (orderRequest.Items == null)
    //        orderRequest.Items = new List<OrderItemDTO>();
    //    else
    //    {
    //        foreach (var item in orderRequest.Items)
    //        {
    //            if (item == null) continue;
    //            item.Name = string.IsNullOrWhiteSpace(item.Name) ? "Item" : item.Name;
    //            item.Quantity = item.Quantity <= 0 ? 1 : item.Quantity;
    //            // تحويل AmountCents إذا كانت بالجنيه (يتم ضربها في 100 لتحويلها للقروش)
    //            item.AmountCents = item.AmountCents <= 0 ? 100 : item.AmountCents;  // افرض 1 جنيه = 100 قرش
    //        }
    //    }

    //    // تجهيز ShippingData: إذا كانت null، يتم تحديد افتراضات
    //    orderRequest.ShippingData ??= new ShippingDataDTO();
    //    orderRequest.ShippingData.FirstName = string.IsNullOrWhiteSpace(orderRequest.ShippingData.FirstName) ? "John" : orderRequest.ShippingData.FirstName;
    //    orderRequest.ShippingData.LastName = string.IsNullOrWhiteSpace(orderRequest.ShippingData.LastName) ? "Doe" : orderRequest.ShippingData.LastName;
    //    orderRequest.ShippingData.Email = string.IsNullOrWhiteSpace(orderRequest.ShippingData.Email) ? "user@example.com" : orderRequest.ShippingData.Email;
    //    orderRequest.ShippingData.PhoneNumber = string.IsNullOrWhiteSpace(orderRequest.ShippingData.PhoneNumber) ? "01000000000" : orderRequest.ShippingData.PhoneNumber;
    //    // 3) تسجيل الطلب (Register Order)
    //    var orderRegistrationResponse =  await _httpClient.PostAsJsonAsync($"{_options.BaseUrl}/api/ecommerce/orders", orderRequest); 
    //    if (orderRegistrationResponse is null || orderRegistrationResponse.Id <= 0)
    //        throw new InvalidOperationException("Failed to register order at Paymob.");

    //    // 4) طلب Payment Key
    //    if (!int.TryParse(orderRequest.AmountCents, out var amountCentsInt))
    //        throw new InvalidOperationException("Invalid AmountCents format. Expected numeric string in cents.");

    //    var paymentKeyRequest = new PaymentKeyRequest
    //    {
    //        AuthToken = auth.Token,
    //        OrderId = orderRegistrationResponse.Id,
    //        AmountCents = amountCentsInt,
    //        Currency = orderRequest.Currency,
    //        IntegrationId = _options.IntegrationIdCard, // أو استخدم _options.IntegrationIdWallet حسب نوع الدفع
    //        Expiration = 3600,  // تحديد مدة الصلاحية بالثواني
    //        BillingData = new Paymob.Net.Models.BillingData
    //        {
    //            FirstName = orderRequest.ShippingData.FirstName,
    //            LastName = orderRequest.ShippingData.LastName,
    //            Email = orderRequest.ShippingData.Email,
    //            PhoneNumber = orderRequest.ShippingData.PhoneNumber,
    //            City = "Cairo",
    //            Country = "EG",
    //            Apartment = "NA",
    //            Floor = "NA",
    //            Street = "NA",
    //            Building = "NA",
    //            ShippingMethod = "NA",
    //            PostalCode = "NA",
    //            State = "C"
    //        }
    //    };

    //    var paymentKeyResponse = await _paymobClient.RequestPaymentKeyAsync(paymentKeyRequest).ConfigureAwait(false);
    //    if (paymentKeyResponse is null || string.IsNullOrWhiteSpace(paymentKeyResponse.Token))
    //        throw new InvalidOperationException("Failed to generate payment key.");

    //    var iframeUrl = $"{_options.BaseUrl}/api/acceptance/iframes/{_options.IframeId}?payment_token={paymentKeyResponse.Token}";

    //    // إرجاع response مع تفاصيل الطلب
    //    return new PaymobOrderResponse
    //    {
    //        OrderId = orderRegistrationResponse.Id.ToString(),
    //        PaymentKey = paymentKeyResponse.Token,
    //        IframeUrl = iframeUrl,
    //        Status = "pending"
    //    };
    //}


    //public async Task<PaymobOrderResponse> RegisterOrderAsync(OrderRegistrationRequestDTO orderRequest)
    //{
    //    if (orderRequest is null)
    //        throw new ArgumentNullException(nameof(orderRequest));

    //    var authResponse = await _httpClient.PostAsJsonAsync($"{_options.BaseUrl}/api/auth/tokens", new { api_key = _options.ApiKey });
    //    var auth = await authResponse.Content.ReadFromJsonAsync<AuthResponse>();

    //    if (auth is null || string.IsNullOrEmpty(auth.Token))
    //        throw new InvalidOperationException("Paymob authentication failed.");

    //    orderRequest.AuthToken = auth.Token;
    //    orderRequest.Currency = string.IsNullOrWhiteSpace(orderRequest.Currency) ? "EGP" : orderRequest.Currency;
    //    orderRequest.DeliveryNeeded = string.IsNullOrWhiteSpace(orderRequest.DeliveryNeeded) ? "false" : orderRequest.DeliveryNeeded;

    //    if (orderRequest.Items == null)
    //        orderRequest.Items = new List<OrderItemDTO>();
    //    else
    //    {
    //        foreach (var item in orderRequest.Items)
    //        {
    //            if (item == null) continue;
    //            item.Name = string.IsNullOrWhiteSpace(item.Name) ? "Item" : item.Name;
    //            item.Quantity = item.Quantity <= 0 ? 1 : item.Quantity;
    //            item.AmountCents = item.AmountCents <= 0 ? 100 : item.AmountCents; // تحويل AmountCents للقروش
    //        }
    //    }

    //    orderRequest.ShippingData ??= new ShippingDataDTO();
    //    orderRequest.ShippingData.FirstName = string.IsNullOrWhiteSpace(orderRequest.ShippingData.FirstName) ? "John" : orderRequest.ShippingData.FirstName;
    //    orderRequest.ShippingData.LastName = string.IsNullOrWhiteSpace(orderRequest.ShippingData.LastName) ? "Doe" : orderRequest.ShippingData.LastName;
    //    orderRequest.ShippingData.Email = string.IsNullOrWhiteSpace(orderRequest.ShippingData.Email) ? "user@example.com" : orderRequest.ShippingData.Email;
    //    orderRequest.ShippingData.PhoneNumber = string.IsNullOrWhiteSpace(orderRequest.ShippingData.PhoneNumber) ? "01000000000" : orderRequest.ShippingData.PhoneNumber;

    //    var orderRegistrationResponse = await _httpClient.PostAsJsonAsync($"{_options.BaseUrl}/api/ecommerce/orders", orderRequest);
    //    if (!orderRegistrationResponse.IsSuccessStatusCode)
    //        throw new InvalidOperationException("Failed to register order at Paymob.");

    //    var orderResponse = await orderRegistrationResponse.Content.ReadFromJsonAsync<OrderRegistrationResponse>();
    //    if (orderResponse is null || orderResponse.Id <= 0)
    //        throw new InvalidOperationException("Failed to register order at Paymob.");

    //    if (!int.TryParse(orderRequest.AmountCents, out var amountCentsInt))
    //        throw new InvalidOperationException("Invalid AmountCents format. Expected numeric string in cents.");

    //    var paymentKeyRequest = new PaymentKeyRequest
    //    {
    //        AuthToken = auth.Token,
    //        OrderId = orderResponse.Id,
    //        AmountCents = amountCentsInt,
    //        Currency = orderRequest.Currency,
    //        IntegrationId = _options.IntegrationIdCard,
    //        Expiration = 3600, 
    //        BillingData = new Paymob.Net.Models.BillingData
    //        {
    //            FirstName = orderRequest.ShippingData.FirstName,
    //            LastName = orderRequest.ShippingData.LastName,
    //            Email = orderRequest.ShippingData.Email,
    //            PhoneNumber = orderRequest.ShippingData.PhoneNumber,
    //            City = "Cairo",
    //            Country = "EG",
    //            Apartment = "NA",
    //            Floor = "NA",
    //            Street = "NA",
    //            Building = "NA",
    //            ShippingMethod = "NA",
    //            PostalCode = "NA",
    //            State = "C"
    //        }
    //    };

    //    var paymentKeyResponse = await _httpClient.PostAsJsonAsync($"{_options.BaseUrl}/api/acceptance/payment_keys", paymentKeyRequest);
    //    if (!paymentKeyResponse.IsSuccessStatusCode)
    //        throw new InvalidOperationException("Failed to generate payment key.");

    //    var paymentKey = await paymentKeyResponse.Content.ReadFromJsonAsync<PaymentKeyResponse>();
    //    if (paymentKey is null || string.IsNullOrWhiteSpace(paymentKey.Token))
    //        throw new InvalidOperationException("Failed to generate payment key.");

    //    var iframeUrl = $"{_options.BaseUrl}/api/acceptance/iframes/{_options.IframeId}?payment_token={paymentKey.Token}";

    //    return new PaymobOrderResponse
    //    {
    //        OrderId = orderResponse.Id.ToString(),
    //        PaymentKey = paymentKey.Token,
    //        IframeUrl = iframeUrl,
    //        Status = "pending"
    //    };
    //}

}
