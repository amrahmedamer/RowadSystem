using FluentValidation;

namespace RowadSystem.Shard.Contract.Address;

public class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
    {
        RuleFor(x => x.GovernorateId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.CityId)
           .NotNull()
           .NotEmpty();

          RuleFor(x => x.Street)
          .MaximumLength(50);

          RuleFor(x => x.AddressDetails)
          .MaximumLength(200);

    }
}
