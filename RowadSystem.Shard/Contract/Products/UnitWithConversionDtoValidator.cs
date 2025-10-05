namespace RowadSystem.Shard.Contract.Products;

public class UnitWithConversionDtoValidator : AbstractValidator<UnitWithConversionDto>
{
    public UnitWithConversionDtoValidator()
    {
        RuleFor(u => u.UnitId)
            .NotEmpty();

        RuleFor(u => u.SellingPrice)
             .NotNull()
            .GreaterThan(0);

        RuleFor(u => u.QuantityInBaseUnit)
            .GreaterThan(0);


    }
}

