namespace RowadSystem.API.Errors;

public static class CategoryErrors
{
    public static Error NotFound = new Error( "Category.NotFound", "The specified category was not found.", StatusCodes.Status404NotFound);

    public static Error AlreadyExists = new Error( "Category.AlreadyExists", "An expense category with the same name already exists.", StatusCodes.Status409Conflict);

    public static Error InvalidName = new Error( "Category.InvalidName", "The expense category name is invalid or empty.", StatusCodes.Status400BadRequest);
}
