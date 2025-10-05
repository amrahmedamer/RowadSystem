namespace RowadSystem.Shard.Contract.Discounts;

public class DiscountRequestValidator : AbstractValidator<DiscountRequest>
{
    public DiscountRequestValidator()
    {

        RuleFor(x => x.Name)
           .NotEmpty()
           .Length(1, 50);

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(1, 200);

        RuleFor(x => x.Value)
            .NotNull()
            .InclusiveBetween(0, 100);

        RuleFor(x => x.StartDate)
            .NotNull()
            .LessThan(x => x.EndDate);

        RuleFor(x => x.EndDate)
            .NotNull()
            .GreaterThan(x => x.StartDate);

    }
}
