namespace RowadSystem.Shard.Contract.Products;

public class BarcodeRequestValidator : AbstractValidator<BarcodeRequest>
{
    public BarcodeRequestValidator()
    {
        RuleFor(x => x.barcode)
            .NotEmpty()
            .Length(1, 50);
    }
}

