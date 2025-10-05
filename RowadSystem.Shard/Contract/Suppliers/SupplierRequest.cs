using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Phones;

namespace RowadSystem.Shard.Contract.Suppliers;

public class SupplierRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public AddressRequest Address { get; set; }
    public List<PhoneNumberRequest>? PhoneNumbers { get; set; }
};
