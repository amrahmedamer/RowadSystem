namespace  RowadSystem.Shard.Abstractions;

public static class OrderErrors
{
    public static Error CustomerAlreadyHasOrder = new Error("CustomerAlreadyHasOrder", "Customer already has an existing order.", StatusCodes.Status409Conflict);
    public static Error UserAlreadyHasOrder = new Error("UserAlreadyHasOrder", "User already has an existing order.", StatusCodes.Status409Conflict);
    public static Error OrderItemsNotFound = new Error("OrderItemsNotFound", "The order does not contain any items.", StatusCodes.Status404NotFound);
    public static Error MustBeLoggedInToCreateOrder = new("Order.MustBeLoggedIn", "You must be logged in to place an order.", StatusCodes.Status401Unauthorized);
    public static Error OrderItemsCannotBeEmpty = new("Order.ItemsEmpty", "An order must contain at least one item.", StatusCodes.Status400BadRequest);
    public static Error SomeProductsNotFound = new("Order.SomeProductsNotFound", "Some of the selected products could not be found.", StatusCodes.Status404NotFound);
    public static readonly Error CannotUpdateOrder = new("Order.CannotUpdateOrder", "Cannot update the order after it has been processed or completed.", StatusCodes.Status400BadRequest);
    public static Error OrderNotFound = new Error("OrderNotFound", "The requested order was not found.", StatusCodes.Status404NotFound);
}
