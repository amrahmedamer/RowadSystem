
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Orders;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Paymob;
using RowadSystem.Shard.Contract.ShoppingCarts;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Payments;

public class PaymentService(IHttpClientFactory httpClient) : IPaymentService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");

    public async Task<Result<List<ShoppingCartResponse>>> GetCartItem()
    {
        var response = await _httpClient.GetAsync("/api/ShoppingCarts");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<ShoppingCartResponse>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<List<ShoppingCartResponse>>(response);
        }
    }
    public async Task<Result<CheckoutResponse>> ProcessToCheckOutAsync(OrderRequest orderRequest)
    {

        var response = await _httpClient.PostAsJsonAsync("/api/ShoppingCarts/check-out", orderRequest);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CheckoutResponse>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<CheckoutResponse>(response);
        }

    }
    public async Task<Result<OrderResponse>> GetOrder()
    {

        var response = await _httpClient.GetAsync("/api/Orders");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<OrderResponse>(response);
        }

    }
    public async Task<Result> UpdatePaymentMethodOrder(PaymentMethodOrder paymentMethodOrder)
    {

        var response = await _httpClient.PutAsJsonAsync("/api/Orders", paymentMethodOrder);

        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<Result>(response);
        }

    }
    public async Task<StartPaymentResponse> Wallet(StartWalletPaymentRequest payload)
    {

        var response = await _httpClient.PostAsJsonAsync("api/Payments/wallet", payload);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<StartPaymentResponse>();
            return result;
        }
        else
        {
            return null;
        }

    }
   
    public async Task<StartPaymentResponse> Card(StartPaymentRequest payload)
    {

        var response = await _httpClient.PostAsJsonAsync("api/Payments/card", payload);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<StartPaymentResponse>();
            return result;
        }
        else
        {
            return null;
        }
    }

}
