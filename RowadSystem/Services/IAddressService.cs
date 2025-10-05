using RowadSystem.Shard.Contract.Address;

namespace RowadSystem.API.Services;

public interface IAddressService
{
    Task<Result<List<GovernrateResponse>>> GetAllGovernrate();
    Task<Result<List<CitiesResponse>>> GetAllCities();
}
