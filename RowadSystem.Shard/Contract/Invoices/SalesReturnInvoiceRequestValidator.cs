using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.Shard.Contract.Invoices;

public class SalesReturnInvoiceRequestValidator : AbstractValidator<SalesReturnInvoiceRequest>
{
    public SalesReturnInvoiceRequestValidator()
    {
        RuleFor(x => x.Notes)
           .NotEmpty()
           .MaximumLength(500)
           .NotNull();

        RuleFor(x => x.Payments)
            .NotEmpty()
            .NotNull();

        //RuleFor(x => x.InvoiceNumber)
        //    .NotEmpty()
        //    .NotNull();

        //RuleFor(x => x.Barcode)
        //    .NotEmpty()
        //    .NotNull();

        RuleForEach(x => x.Payments)
            .SetValidator(new PaymentRequestValidator());

        RuleFor(x => x.Items)
            .NotEmpty()
            .NotNull();

        RuleForEach(x => x.Items)
            .SetValidator(new SalesReturnItemRequestValidator());

        RuleFor(x => x.SalesInvoiceId)
            .NotNull()
            .NotEmpty();


    }

}
