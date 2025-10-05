namespace RowadSystem.API.Errors;

public static class CashierHandoverErrors
{
    public static Error NotFound = new Error("CashierHandover.NotFound", "The specified cashier handover record was not found.", StatusCodes.Status404NotFound);

    public static Error MismatchedAmount = new Error("CashierHandover.MismatchedAmount", "The amount in the drawer does not match the total sales amount.", StatusCodes.Status400BadRequest);

    public static Error LateHandover = new Error("CashierHandover.LateHandover", "The cashier handover is late.", StatusCodes.Status400BadRequest);

    public static Error InvalidAmount = new Error("CashierHandover.InvalidAmount", "The amount in the drawer is invalid or empty.", StatusCodes.Status400BadRequest);

    public static Error CashierHandoverWithDifference = new Error("CashierHandover.CashierHandoverWithDifference", "The cashier handover contains a mismatch between the drawer and the sales amount.", StatusCodes.Status400BadRequest);

    public static Error InvalidDate = new Error("CashierHandover.InvalidDate", "The handover date is invalid or missing.", StatusCodes.Status400BadRequest);

    public static Error DuplicateHandoverInSameDay = new Error("CashierHandover.DuplicateHandoverInSameDay","You have already handed over today. Please try again tomorrow.",StatusCodes.Status400BadRequest);


    public static Error HandoverFailed = new Error("CashierHandover.HandoverFailed", "The cashier handover operation failed. Please try again later.", StatusCodes.Status500InternalServerError);
}
