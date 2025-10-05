using RowadSystem.Shard.Contract.Brands;

namespace RowadSystem.Services;

public interface IBrandService
{
    Task<Result> AddBrandAsync(BrandRequset requset);
    Task<Result<List<BrandResponse>>> GetAllBrandsAsync();
}
