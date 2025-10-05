using RowadSystem.Shard.Contract.Phones;
using System.ComponentModel;

namespace RowadSystem.Shard.Contract.Users;

public class UserResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? PhoneNumbers { get; set; }
};