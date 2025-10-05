using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.ShoppingCarts;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.ShoppingCart;

public class CartService(IHttpClientFactory httpClient) : ICartService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");
    public async Task<HttpResponseMessage> AddCartItem(ShoppingCartItemRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/ShoppingCarts", request);

        return response;
    }
    public async Task<HttpResponseMessage> UpdateQuantityCartItem(UpdateQuantityRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync("/api/ShoppingCarts/update-item-quantity", request);

        return response;
    }
    public async Task<List<ShoppingCartResponse>> GetCartItem()
    {
        var response = await _httpClient.GetAsync("/api/ShoppingCarts");

        var result = await response.Content.ReadFromJsonAsync<List<ShoppingCartResponse>>();

        return result ?? new List<ShoppingCartResponse>();
    }
    public async Task<HttpResponseMessage> RemoveItem(int itemId)
    {
        var response = await _httpClient.DeleteAsync($"/api/ShoppingCarts/remove-item/{itemId}");

        return response;
    }
    public async Task<Result<int>> CountItem()
    {
        var response = await _httpClient.GetAsync("/api/ShoppingCarts/count");
        if (response.IsSuccessStatusCode)
        {
            var result =await response.Content.ReadFromJsonAsync<int>();
            return Result.Success(result);
        }

        return Result.Failure<int>(new Error (code:"Count",description:"Invalid Count",500));

    }





}
