namespace RowadSystem.Shard.Contract.Invoices;

public class SalesInvoiceItemRequestValidator : AbstractValidator<SalesInvoiceItemRequest>
{
    public SalesInvoiceItemRequestValidator()
    {

        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.UnitId)
            .NotEmpty();
    }
}
