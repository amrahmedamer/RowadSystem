using RowadSystem.Shard.Contract.Acounts;
using RowadSystem.Shard.Contract.Products;
using RowadSystem.Shard.Contract.Users;

namespace RowadSystem.Mapping;

public class MappingConfigrations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email.Split('@', StringSplitOptions.None)[0]);

        config.NewConfig<UserRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email.Split('@', StringSplitOptions.None)[0])
            .Map(dest => dest.EmailConfirmed, src => true);

        config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
            .Map(dest => dest.Id, src => src.user.Id)
            .Map(dest => dest.Email, src => src.user.Email)
            .Map(dest => dest.FirstName, src => src.user.FirstName)
            .Map(dest => dest.LastName, src => src.user.LastName)
            .Map(dest => dest.Roles, src => src.roles);


        config.NewConfig<(ApplicationUser user, IList<string> roles), UserPorfileResponse>()
             .Map(dest => dest.Id, src => src.user.Id)
            .Map(dest => dest.Email, src => src.user.Email)
            .Map(dest => dest.UserName, src => src.user.UserName)
            .Map(dest => dest.FirstName, src => src.user.FirstName)
            .Map(dest => dest.IsDeleted, src => src.user.IsDeleted)
            .Map(dest => dest.LastName, src => src.user.LastName)
            .Map(dest => dest.Roles, src => src.roles);

        config.NewConfig<ProductRequest, Product>()
            .Ignore(x => x.Images)
            .Ignore(x => x.Barcode);

        config.NewConfig<UpdateProductRequest, Product>()
            .Ignore(x => x.Images)
            .Ignore(x => x.Barcode);

    }
}
