namespace RowadSystem.Shard.Contract.Invoices;

public class SalesReturnItemRequestValidator : AbstractValidator<SalesReturnItemRequest>
{
    public SalesReturnItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.UnitId)
            .NotEmpty()
            .NotNull();
    }
}
