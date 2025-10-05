namespace  RowadSystem.Shard.Abstractions;

public static class SupplierErrors
{
    public static Error SupplierAlreadyExists = new Error("SupplierAlreadyExists", "A supplier with the provided details already exists.", StatusCodes.Status409Conflict);
    public static Error SupplierNotFound = new Error("SupplierNotFound", "Supplier not found.", StatusCodes.Status404NotFound);

}
