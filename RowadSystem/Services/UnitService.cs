using RowadSystem.Shard.Contract.Units;

namespace RowadSystem.API.Services;

public class UnitService(ApplicationDbContext context) : IUnitService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<List<UnitResponse>>> GetUnitsAsync()
    {
        var units = await _context.Units.ToListAsync();

        if (units is null || !units.Any())
            return Result.Failure<List<UnitResponse>>(ProductErrors.UnitNotFound);

        var respons = units.Select(u => new UnitResponse
        {
            Id = u.Id,
            Name = u.Name
        }).ToList();

        return Result.Success(respons);
    }
}
