namespace RowadSystem.Shard.Contract.Phones;

public class PhoneNumberRequestValidator : AbstractValidator<PhoneNumberRequest>
{

    public PhoneNumberRequestValidator()
    {
        RuleFor(x => x.Number)
            .NotNull()
            .NotEmpty()
            .Matches(@"^01[0125][0-9]{8}$");
    }
}
