namespace  RowadSystem.Shard.Abstractions;

public static class ShoppingCartErrors
{
    public static Error NotFound = new Error("ShoppingCart.NotFound", "The requested shopping cart was not found.", StatusCodes.Status404NotFound);

    public static Error ItemNotFound = new Error("ShoppingCart.ItemNotFound", "The requested item in the shopping cart was not found.", StatusCodes.Status404NotFound);

    public static Error ItemAlreadyExists = new Error("ShoppingCart.ItemAlreadyExists", "The item already exists in the shopping cart.", StatusCodes.Status409Conflict);

    public static Error EmptyCart = new Error("ShoppingCart.Empty", "The shopping cart is empty.", StatusCodes.Status400BadRequest);
    public static Error MissingUserOrGuest = new("ShoppingCart.MissingUserOrGuest", "User ID or Guest ID is required to checkout the cart.", StatusCodes.Status401Unauthorized);

    public static Error MustBeLoggedInToCheckout = new("ShoppingCart.MustBeLoggedIn", "Please log in to be able to checkout.", StatusCodes.Status401Unauthorized);
    public static Error InvalidQuantity = new Error("ShoppingCart.InvalidQuantity", "The quantity provided is invalid.", StatusCodes.Status400BadRequest);
}
