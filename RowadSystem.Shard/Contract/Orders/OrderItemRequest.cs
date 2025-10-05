namespace RowadSystem.Shard.Contract.Orders;

public record OrderItemRequest
(
    int ProductId,
    int Quantity,
    int UnitId
 );

