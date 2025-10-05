using RowadSystem.Shard.Contract.Coupons;

namespace RowadSystem.Services;

public interface ICouponService
{
    Task<Result> AddCoupon(CouponRequest request);
}
