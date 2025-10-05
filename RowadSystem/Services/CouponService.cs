using RowadSystem.Shard.Contract.Coupons;

namespace RowadSystem.Services;

public class CouponService(ApplicationDbContext context) : ICouponService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddCoupon(CouponRequest request)
    {

        if (await _context.Coupons.AnyAsync(d => d.Code == request.Code && d.StartDate <= request.EndDate && d.EndDate >= request.StartDate))
            return Result.Failure(CouponErrors.CouponAlreadyExists);

        var coupon = request.Adapt<Coupon>();
        await _context.Coupons.AddAsync(coupon);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}
