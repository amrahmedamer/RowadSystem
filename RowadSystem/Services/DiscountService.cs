using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Discounts;

namespace RowadSystem.Services;

public class DiscountService(ApplicationDbContext context) : IDiscountService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddDiscount(DiscountRequest request)
    {
        if (await _context.Discounts.AnyAsync(d => d.Name == request.Name && d.StartDate <= request.EndDate && d.EndDate >= request.StartDate))
            return Result.Failure(DiscountErrors.DiscountAlreadyExists);

        var discount = request.Adapt<Discount>();
        await _context.Discounts.AddAsync(discount);
        await _context.SaveChangesAsync();
        return Result.Success();

    }
    public async Task<Result<List<DiscountResponse>>> GetAllDiscounts()
    {
        var discounts = await _context.Discounts.Select(c => new DiscountResponse
        {
            Id = c.Id,
            Name = c.Name
        }).ToListAsync();

        //if (!discounts.Any())
        //    return Result.Failure<List<DiscountResponse>>(DiscountErrors.DiscountNotFound);

        return Result.Success(discounts);
    }

    public async Task<Result> UpdateDiscount(int id, DiscountRequest request)
    {
        if (await _context.Discounts.FindAsync(id) is not { } discount)
            return Result.Failure(DiscountErrors.DiscountNotFound);

        _context.Discounts.Update(request.Adapt(discount));
        await _context.SaveChangesAsync();

        return Result.Success();
    }
    public async Task<Result> AssignDiscountToProduct(AssignDiscountToProductRequest request)
    {
        if (await _context.Discounts.FindAsync(request.DiscountId) is not { } discount)
            return Result.Failure(DiscountErrors.DiscountNotFound);

        var products = await _context.Products
            .Where(x => request.ProductIds.Contains(x.Id))
            .ToListAsync();

        if (!products.Any())
            return Result.Failure(ProductErrors.ProductNotFound);

        foreach (var product in products)
            product.DiscountId = discount.Id;

        await _context.SaveChangesAsync();
        return Result.Success();
    }



}

