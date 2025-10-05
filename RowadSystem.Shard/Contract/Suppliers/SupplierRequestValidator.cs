using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Phones;

namespace RowadSystem.Shard.Contract.Suppliers;

public class SupplierRequestValidator : AbstractValidator<SupplierRequest>
{
    public SupplierRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Address)
            .SetValidator(new AddressRequestValidator());

        RuleForEach(x => x.PhoneNumbers)
            .SetValidator(new PhoneNumberRequestValidator());


    }
}

