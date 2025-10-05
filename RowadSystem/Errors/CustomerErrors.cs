namespace  RowadSystem.Shard.Abstractions;

public static class CustomerErrors
{
    public static Error CustomerAlreadyExists = new Error("CustomerAlreadyExists", "A customer with the provided details already exists.", StatusCodes.Status409Conflict);

    public static Error CustomerNotFound = new Error("CustomerNotFound", "Customer not found.", StatusCodes.Status404NotFound);


    public static readonly Error InvalidPhoneNumbers = new Error("Customer.InvalidPhoneNumbers", "One or more phone numbers are invalid.", StatusCodes.Status403Forbidden);

    public static readonly Error InvalidAddress = new("Customer.InvalidAddress", "The provided address is invalid.", StatusCodes.Status403Forbidden);

}
