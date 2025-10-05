using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.Shard.Contract.Invoices;

public class PurchaseInvoiceRequestValidator : AbstractValidator<PurchaseInvoiceRequest>
{

    public PurchaseInvoiceRequestValidator()
    {


        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Payments)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Payments)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.SupplierId)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Payments)
            .SetValidator(new PaymentRequestValidator());

        RuleForEach(x => x.Items)
            .SetValidator(new PurchaseInvoiceItemRequestValidator());
    }
}
