namespace RowadSystem.Shard.Contract.Acounts;

public class UpdateUserProfileRequestValidator : AbstractValidator<UpdateUserProfileRequest>
{

    public UpdateUserProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^01[0125][0-9]{8}$").WithMessage("Phone number must be a valid international format.");
    }
}
