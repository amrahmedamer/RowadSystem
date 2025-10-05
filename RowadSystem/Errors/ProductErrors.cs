namespace  RowadSystem.Shard.Abstractions;

public static class ProductErrors
{
    public static Error ProductAlreadyExists = new Error("ProductAlreadyExists", "Product already exists with the same name, brand, category, and supplier.", StatusCodes.Status409Conflict);
    public static Error ProductAlreadyExistsByBarcode = new Error("ProductAlreadyExistsByBarcode", "Product already exists with the same barcode.", StatusCodes.Status409Conflict);
    public static Error NoUnitsProvided = new Error("NoUnitsProvided", "At least one unit must be provided for the product.", StatusCodes.Status400BadRequest);
    public static Error ProductNotFound = new Error("ProductNotFound", "The requested product was not found.", StatusCodes.Status404NotFound);
    public static Error ImageDeletionFailed = new Error("ImageDeletionFailed", "Failed to delete the product image from the server.", StatusCodes.Status500InternalServerError);
    public static Error MissingInventory = new("Product.MissingInventory", "No inventory information found for the selected product.", StatusCodes.Status400BadRequest);
    public static Error NotEnoughStock = new("Product.NotEnoughStock", "The requested quantity exceeds the available stock.", StatusCodes.Status400BadRequest);
    public static Error MissingSellingPrice = new("Product.MissingSellingPrice", "The product has no defined selling price for the selected unit.", StatusCodes.Status400BadRequest);
    public static Error ProductQuantityNotAvailable = new("Product.QuantityNotAvailable", "The requested quantity for the product is not available in stock.", StatusCodes.Status400BadRequest);
    public static Error ExceedsRemainingQuantityToReturn = new("Product.ExceedsRemainingQuantityToReturn", "The return quantity exceeds the remaining quantity that can be returned from the original invoice.", StatusCodes.Status400BadRequest);
    public static Error ProductNotFoundInInvoice = new("Product.NotFoundInInvoice", "The specified product was not found in the original invoice.", StatusCodes.Status400BadRequest);
    public static readonly Error NoProductsInInvoice = new("Product.NoProductsInInvoice", "The original invoice does not contain any products.", StatusCodes.Status400BadRequest);

    public static Error ProductUnitsNotFound = new("Product.UnitsNotFound", "No units were found for the specified product.", StatusCodes.Status404NotFound);
    public static Error UnitNotFound = new("ShoppingCart.UnitNotFound", "The selected unit is not valid for this product.", 400);
    public static Error InvalidQuantity = new("ShoppingCart.InvalidQuantity", "Quantity must be greater than zero.", 400);
}

