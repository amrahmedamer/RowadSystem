namespace  RowadSystem.Shard.Abstractions;

public static class DiscountErrors
{
    public static Error DiscountNotFound = new Error("DiscountNotFound", "The requested discount was not found.", StatusCodes.Status404NotFound);
    public static Error DiscountAlreadyExists = new Error("DiscountAlreadyExists", "A discount with the same values already exists.", StatusCodes.Status409Conflict);

}
