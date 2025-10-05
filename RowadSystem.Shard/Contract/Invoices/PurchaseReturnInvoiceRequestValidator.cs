using RowadSystem.Shard.Contract.Payments;

namespace RowadSystem.Shard.Contract.Invoices;

public class PurchaseReturnInvoiceRequestValidator : AbstractValidator<PurchaseReturnInvoiceRequest>
{
    public PurchaseReturnInvoiceRequestValidator()
    {
        RuleFor(x => x.PurchaseInvoiceId)
            .GreaterThan(0)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.SupplierId)
            .GreaterThan(0)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Notes)
            .NotEmpty()
            .MaximumLength(500)
            .NotNull();

        RuleFor(x => x.Payments)
            .NotEmpty()
            .NotNull();

        RuleForEach(x => x.Payments)
            .SetValidator(new PaymentRequestValidator());

        RuleFor(x => x.Items)
            .NotEmpty()
            .NotNull();

        RuleForEach(x => x.Items)
            .SetValidator(new PurchaseReturnInvoiceItemRequestValidator());

        //RuleFor(x => x.InvoiceNumber)
        // .NotEmpty()
        // .NotNull();
        //RuleFor(x => x.Barcode)
        //    .NotEmpty()
        //    .NotNull();
    }
}
