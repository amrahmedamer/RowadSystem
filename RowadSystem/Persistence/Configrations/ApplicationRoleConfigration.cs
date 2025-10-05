using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RowadSystem.Shard.consts;

namespace RowadSystem.Persistence.Configrations;

public class ApplicationRoleConfigration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([new ApplicationRole
        {
            Id = DefaultRole.AdminRoleId,
            Name = DefaultRole.Admin,
            NormalizedName = DefaultRole.Admin.ToUpper(),
            ConcurrencyStamp = DefaultRole.AdminRoleConcurrencyStamp,
            IsActive = true,
            IsDefault = false
        },
        new ApplicationRole
        {
            Id = DefaultRole.MemberRoleId,
            Name = DefaultRole.Member,
            NormalizedName = DefaultRole.Member.ToUpper(),
            ConcurrencyStamp = DefaultRole.MemberRoleConcurrencyStamp,
            IsActive = true,
            IsDefault = true
        }
        ]);



    }
}
