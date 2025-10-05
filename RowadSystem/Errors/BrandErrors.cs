namespace RowadSystem.API.Errors;

public static class BrandErrors
{
    public static Error NotFound = new Error("Brand.NotFound", "The specified brand was not found.", StatusCodes.Status404NotFound);

    public static Error AlreadyExists = new Error("Brand.AlreadyExists", "A brand with the same name already exists.", StatusCodes.Status409Conflict);

    public static Error InvalidName = new Error("Brand.InvalidName", "The brand name is invalid or empty.", StatusCodes.Status400BadRequest);
}
