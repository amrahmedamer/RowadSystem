using RowadSystem.Shard.Contract.Address;

namespace RowadSystem.API.Services;

public class AddressService(ApplicationDbContext context) : IAddressService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<List<GovernrateResponse>>> GetAllGovernrate()
    {
        var result = await _context.Governorates.Select(x => new GovernrateResponse { Id = x.Id, Name = x.Name }).ToListAsync();
        return Result.Success(result);
    }
    public async Task<Result<List<CitiesResponse>>> GetAllCities()
    {
        var result = await _context.Cities.Select(x => new CitiesResponse { Id = x.Id, Name = x.Name, GovernorateId = x.GovernorateId }).ToListAsync();
        return Result.Success(result);
    }
}
