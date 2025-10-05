namespace  RowadSystem.Shard.Abstractions;

public static class InvoiceErrors
{
    public static readonly Error InvoiceNotFound = new("Invoice.NotFound", "The invoice was not found.", StatusCodes.Status404NotFound);

    public static readonly Error InvoiceAlreadyExists = new("Invoice.AlreadyExists", "An invoice with the same number already exists.", StatusCodes.Status409Conflict);

    public static readonly Error InvalidInvoiceTotal = new("Invoice.InvalidTotal", "The invoice total amount is invalid.", StatusCodes.Status400BadRequest);

    public static readonly Error EmptyInvoiceItems = new("Invoice.EmptyItems", "The invoice must contain at least one item.", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidCustomerId = new("Invoice.InvalidCustomerId", "The provided customer ID is invalid.", StatusCodes.Status400BadRequest);

    public static readonly Error ReturnedQuantityExceedsSold = new("Invoice.ReturnedQuantityExceedsSold", "Returned quantity exceeds sold quantity.", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidPaymentMethod = new("Invoice.InvalidPaymentMethod", "The selected payment method is not supported.", StatusCodes.Status400BadRequest);

    public static readonly Error InvoiceIsAlreadyPaid = new("Invoice.AlreadyPaid", "This invoice has already been paid.", StatusCodes.Status400BadRequest);

    public static readonly Error UnauthorizedInvoiceAccess = new("Invoice.UnauthorizedAccess", "You are not authorized to access this invoice.", StatusCodes.Status403Forbidden);
    public static readonly Error PaymentAmountExceedsInvoiceTotal = new("Invoice.PaymentAmountExceedsInvoiceTotal", "The payment amount exceeds the total due on this invoice.", StatusCodes.Status400BadRequest);

    public static Error InvalidPayer = new Error("Invoice.InvalidPayer", "Invoice must be linked to either a customer or a user, not both or neither.", StatusCodes.Status400BadRequest);

}
