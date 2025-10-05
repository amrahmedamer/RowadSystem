using RowadSystem.Shard.Contract.Address;
using System.Net.Http;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Addresses;

public class AddressService(IHttpClientFactory httpClientFactory) : IAddressService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("NoAuth");
    public async Task<List<GovernrateResponse>> GetGovernratesAsync()
    {
        var response = await _httpClient.GetAsync("/api/Addresses/Governrates");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<GovernrateResponse>>();
        }

        return new List<GovernrateResponse>();
    }
    public async Task<List<CitiesResponse>> GetCitiesAsync()
    {
        var response = await _httpClient.GetAsync("/api/Addresses/Cities");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<CitiesResponse>>();
        }

        return new List<CitiesResponse>();
    }
}
