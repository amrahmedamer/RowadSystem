namespace RowadSystem.Shard.Contract.Acounts;

public class UserPorfileResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> Roles { get; set; }
    public bool IsDeleted { get; set; }
};
