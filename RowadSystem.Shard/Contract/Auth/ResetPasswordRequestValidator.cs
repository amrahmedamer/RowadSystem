namespace RowadSystem.Shard.Contract.Auth;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Code)
             .NotEmpty()
             .Matches(@"^\d+$").WithMessage("Code must be a 6-digit number.")
             .Length(6);

        RuleFor(x => x.NewPassword)
        .NotEmpty()
        .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._\-]).{6,}$");
    }
}
