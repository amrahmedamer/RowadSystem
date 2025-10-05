using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Phones;

namespace RowadSystem.Shard.Contract.Suppliers;

public class SupplierResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public AddressRequest Addresses { get; set; }=new ();
    public List<PhoneNumberRequest> PhoneNumbers { get; set; } = new();
};
