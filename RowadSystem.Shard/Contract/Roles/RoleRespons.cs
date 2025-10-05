namespace RowadSystem.Shard.Contract.Roles;

public class RoleRespons
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<string> Permissions { get; set; }
};
