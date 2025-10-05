using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Phones;
using RowadSystem.Shard.Contract.Roles;

namespace RowadSystem.Shard.Contract.Users;

public class UserRequest
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public AddressRequest Address { get; set; }
    public List<PhoneNumberRequest> PhoneNumbers { get; set; }
    public List<RoleRespons> Roles { get; set; }
};

