namespace RowadSystem.API.Errors;

public static class ExpenseCategoryErrors
{
    public static Error NotFound = new Error( "ExpenseCategory.NotFound", "The specified expense category was not found.", StatusCodes.Status404NotFound);

    public static Error AlreadyExists = new Error( "ExpenseCategory.AlreadyExists", "An expense category with the same name already exists.", StatusCodes.Status409Conflict);

    public static Error InvalidName = new Error( "ExpenseCategory.InvalidName", "The expense category name is invalid or empty.", StatusCodes.Status400BadRequest);
}
