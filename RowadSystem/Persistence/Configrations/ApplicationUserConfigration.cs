using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RowadSystem.Shard.consts;

namespace RowadSystem.Persistence.Configrations;

public class ApplicationUserConfigration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired(false)
            .HasMaxLength(50);


        builder.HasMany(u => u.Otp)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);

        builder.OwnsMany(u => u.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner();

        var userName = DefaultUser.AdminEmail.Split('@')[0];
        var hasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(
            new ApplicationUser
            {
                Id = DefaultUser.AdminId,
                UserName = userName,
                Email = DefaultUser.AdminEmail,
                NormalizedEmail = DefaultUser.AdminEmail.ToUpper(),
                EmailConfirmed = true,
                FirstName = DefaultUser.AdminFirstName,
                LastName = DefaultUser.AdminLastName,
                NormalizedUserName = userName.ToUpper(),
                PasswordHash = hasher.HashPassword(null!, DefaultUser.AdminPassword),
                ConcurrencyStamp = DefaultUser.AdminConcurencyStamp,
                SecurityStamp = DefaultUser.AdminSecurityStamp

            });
    }
}
