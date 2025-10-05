using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Cashiers;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.CashierHandover;

public class CashierHandoverService(IHttpClientFactory httpClientFactory): ICashierHandoverService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AuthorizedClient");

    public async Task<Result> CashierHandoverAsync(CashierHandoverDTO request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/CashierHandovers/handover", request);
        if (response.IsSuccessStatusCode)
        {
            //var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return Result.Success();
        }
        else
        {

            Console.WriteLine("CashierHandovers faild");
            return await PorblemDetailsExtensions.HandleErrorResponse<Result>(response);
        }
    }
}
