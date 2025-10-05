namespace RowadSystem.Shard.Contract.ShoppingCarts;

public record ShoppingCartRequest
(
  string? userId,
  string? guestId
);
