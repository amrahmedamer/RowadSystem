using RowadSystem.API.Errors;
using RowadSystem.Shard.Contract.Brands;

namespace RowadSystem.Services;

public class BrandService(ApplicationDbContext context) : IBrandService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddBrandAsync(BrandRequset requset)
    {
        var brand = new Brand
        {
            Name = requset.Name,
            Description = requset.Description
        };
        await _context.Brands.AddAsync(brand);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<List<BrandResponse>>> GetAllBrandsAsync()
    {
        var brands = await _context.Brands.ToListAsync();

        if (brands == null || !brands.Any())
            return Result.Failure<List<BrandResponse>>(BrandErrors.NotFound);

        var response = brands.Select(b => new BrandResponse
        (
            b.Id,
            b.Name
        )).ToList();

        return Result.Success(response);
    }
}
