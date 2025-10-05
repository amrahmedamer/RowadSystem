using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RowadSystem.Shard.consts;

namespace RowadSystem.Persistence.Configrations;

public class RoleClaimsConfigration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var permission = Permissions.GetAllPermissions();
        var adminRoleClaim = new List<IdentityRoleClaim<string>>();

        for (var i = 0; i < permission.Count; i++)
        {
            adminRoleClaim.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                RoleId = DefaultRole.AdminRoleId,
                ClaimType = Permissions.Type,
                ClaimValue = permission[i]
            });

        }
        ;

        builder.HasData(adminRoleClaim);

    }
}
