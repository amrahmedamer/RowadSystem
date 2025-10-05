namespace RowadSystem.Entity;

public class ApplicationRole : IdentityRole
{
    public bool IsActive { get; set; } = true;
    public bool IsDefault { get; set; } = false;
}
