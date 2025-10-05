using RowadSystem.Shard.Contract.Units;

namespace RowadSystem.API.Services;

public interface IUnitService
{
    Task<Result<List<UnitResponse>>> GetUnitsAsync();
}
