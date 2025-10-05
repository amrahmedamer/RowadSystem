namespace RowadSystem.Shard.Contract.Products;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .Length(3, 100);

        //RuleFor(p => p.Images)
        //    .NotEmpty();

        //RuleFor(p => p.PurchasePrice)
        //    .GreaterThan(0);
        //RuleFor(p => p.Quantity)
        //    .GreaterThanOrEqualTo(0);

        RuleFor(p => p.Description)
            .MaximumLength(500);

        RuleFor(p => p.Barcode)
            .MaximumLength(30);

        RuleForEach(p => p.UnitsWithConversion)
            .SetValidator(new UnitWithConversionDtoValidator());
;
    }
}
