namespace RowadSystem.Shard.Contract.Auth;

public class otpRequestValidator : AbstractValidator<OtpRequest>
{
    public otpRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}

