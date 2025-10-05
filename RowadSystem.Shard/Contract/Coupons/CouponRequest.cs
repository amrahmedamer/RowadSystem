namespace RowadSystem.Shard.Contract.Coupons;

public record CouponRequest
(
    string Code,
    decimal DiscountValue,
    bool IsPercentage,
    DateTime StartDate,
    DateTime EndDate
    );