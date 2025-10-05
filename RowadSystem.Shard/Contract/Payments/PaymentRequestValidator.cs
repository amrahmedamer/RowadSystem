namespace RowadSystem.Shard.Contract.Payments;

public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
    public PaymentRequestValidator()
    {
        RuleFor(x => x.Amount)
            .InclusiveBetween(0.01m, decimal.MaxValue)
            .GreaterThan(0)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Method)
            .NotEmpty()
            .NotNull();

    }

}
