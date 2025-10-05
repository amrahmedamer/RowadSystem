using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RowadSystem.Shard.consts;

namespace RowadSystem.Persistence.Configrations;

public class UserRolesConfigration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {

        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = DefaultRole.AdminRoleId,
                UserId = DefaultUser.AdminId
            }
        );

    }
}
