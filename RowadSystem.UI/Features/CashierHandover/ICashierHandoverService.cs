using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Cashiers;

namespace RowadSystem.UI.Features.CashierHandover;

public interface ICashierHandoverService
{
    Task<Result> CashierHandoverAsync(CashierHandoverDTO request);
}
