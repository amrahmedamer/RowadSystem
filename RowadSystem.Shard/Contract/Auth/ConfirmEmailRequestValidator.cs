namespace RowadSystem.Shard.Contract.Auth;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{

    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Code)
            .NotEmpty()
            .Matches(@"^\d+$").WithMessage("Code must be a 6-digit number.")
            .Length(6);
    }
}
