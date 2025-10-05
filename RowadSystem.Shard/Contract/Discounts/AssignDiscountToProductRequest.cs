namespace RowadSystem.Shard.Contract.Discounts;

public record AssignDiscountToProductRequest
(
    int DiscountId,
    List<int> ProductIds
 );
