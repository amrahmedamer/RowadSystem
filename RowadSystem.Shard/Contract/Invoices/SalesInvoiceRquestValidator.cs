using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.Shard.Contract.Invoices;

public class SalesInvoiceRquestValidator : AbstractValidator<SalesInvoiceRequest>
{
    public SalesInvoiceRquestValidator()
    {
        
        RuleFor(x => x.Payments)
            .NotEmpty()
            .NotNull();
    
        RuleForEach(x => x.Payments)
            .SetValidator(new PaymentRequestValidator());

        RuleFor(x => x.Items)
            .NotEmpty()
            .NotNull();

        RuleForEach(x => x.Items)
            .SetValidator(new SalesInvoiceItemRequestValidator());


    }
}

