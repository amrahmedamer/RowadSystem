namespace RowadSystem.Shard.Contract.Discounts;

public record DiscountRequest
(
    string Name,
    string Description,
    decimal Value,
    DateTime StartDate,
    DateTime EndDate,
    bool IsPercentage

);
