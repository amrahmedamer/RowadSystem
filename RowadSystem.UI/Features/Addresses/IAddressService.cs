using RowadSystem.Shard.Contract.Address;

namespace RowadSystem.UI.Features.Addresses;

public interface IAddressService
{
    Task<List<GovernrateResponse>> GetGovernratesAsync();
    Task<List<CitiesResponse>> GetCitiesAsync();
}
