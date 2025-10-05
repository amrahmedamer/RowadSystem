namespace RowadSystem.Shard.Abstractions;

public static class CouponErrors
{
    public static Error CouponNotFound = new Error("CouponNotFound", "The requested coupon was not found.", StatusCodes.Status404NotFound);
    public static Error CouponAlreadyExists = new Error("CouponAlreadyExists", "A coupon with the same code or values already exists.", StatusCodes.Status409Conflict);
    public static Error CouponInvalidOrExpired = new Error("CouponInvalidOrExpired", "The coupon is either invalid or has expired.", StatusCodes.Status400BadRequest);




}
