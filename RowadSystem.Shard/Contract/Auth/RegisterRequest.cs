using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Phones;

namespace RowadSystem.Shard.Contract.Auth;

//public record RegisterRequest
//(
//    string FirstName,
//    string LastName,
//    string Email,
//    string Password,
//    AddressRequest Address,
//    List<PhoneNumberRequest> PhoneNumbers
//);

public class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public AddressRequest Address { get; set; } = new();
    public List<PhoneNumberRequest> PhoneNumbers { get; set; } = new()
    {
         new PhoneNumberRequest { Number = string.Empty, IsPrimary = true }
    };

}
