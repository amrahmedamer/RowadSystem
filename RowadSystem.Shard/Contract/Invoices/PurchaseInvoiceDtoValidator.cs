


namespace RowadSystem.Shard.Contract.Invoices;
internal class PurchaseInvoiceDtoValidator : AbstractValidator<PurchaseInvoiceDto>
{
    public PurchaseInvoiceDtoValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Quantity)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);
       
    }
}