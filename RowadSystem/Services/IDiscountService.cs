using RowadSystem.Shard.Contract.Discounts;

namespace RowadSystem.Services;

public interface IDiscountService
{
    Task<Result> AddDiscount(DiscountRequest request);
    Task<Result> UpdateDiscount(int id, DiscountRequest request);
    Task<Result> AssignDiscountToProduct(AssignDiscountToProductRequest request);
    Task<Result<List<DiscountResponse>>> GetAllDiscounts();
}
