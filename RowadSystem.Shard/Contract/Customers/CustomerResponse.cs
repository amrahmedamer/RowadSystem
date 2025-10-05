using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Phones;

namespace RowadSystem.Shard.Contract.Customers;

//public record CustomerResponse
//(
//    int Id,
//    string Name,
//    string Email,
//    AddressRequest Addresses,
//    List<PhoneNumberRequest> PhoneNumbers
//);


public class CustomerResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public AddressRequest Addresses { get; set; }
    public List<PhoneNumberRequest> PhoneNumbers { get; set; }
}
