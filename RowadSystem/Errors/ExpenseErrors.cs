namespace RowadSystem.API.Errors;

public static class ExpenseErrors
{
    public static Error NotFound = new Error( "Expense.NotFound","The specified expense was not found.",StatusCodes.Status404NotFound);

    public static Error InvalidAmount = new Error("Expense.InvalidAmount","The expense amount must be greater than zero.",StatusCodes.Status400BadRequest);

    public static Error InvalidCategory = new Error("Expense.InvalidCategory", "The specified expense category is invalid or does not exist.", StatusCodes.Status400BadRequest);

    public static Error InvalidPaymentMethod = new Error( "Expense.InvalidPaymentMethod","The specified payment method is invalid.",StatusCodes.Status400BadRequest);
}
