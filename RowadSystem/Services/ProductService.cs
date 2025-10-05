using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Helpers;
using RowadSystem.Entity;
using RowadSystem.Helpers;
using RowadSystem.Shard.Contract.Barcode;
using RowadSystem.Shard.Contract.Discounts;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Image;
using RowadSystem.Shard.Contract.Products;

namespace RowadSystem.Services;

public class ProductService(ApplicationDbContext context,
    IImageService imageService,
    IBarcodeService barcodeService) : IProductService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IImageService _imageService = imageService;
    private readonly IBarcodeService _barcodeService = barcodeService;

    public async Task<Result<int>> AddProductAsync(string createByUserId, ProductRequest request)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {

            if (await _context.Products.AnyAsync(p =>
            p.Name == request.Name
            && p.BrandId == request.BrandId
            && p.CategoryId == request.CategoryId))
                return Result.Failure<int>(ProductErrors.ProductAlreadyExists);

            if (await _context.Products.AnyAsync(p => p.Barcode == request.Barcode && p.Barcode != null))
                return Result.Failure<int>(ProductErrors.ProductAlreadyExistsByBarcode);

            var product = request.Adapt<Product>();
            product.CreatedBy = createByUserId;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            product.Barcode = request.Barcode is null ? Ean13Generator.Generate(product.Id) : request.Barcode;

            if (request.UnitsWithConversion is null || !request.UnitsWithConversion.Any())
                return Result.Failure<int>(ProductErrors.NoUnitsProvided);

            var productUnits = request.UnitsWithConversion.Adapt<List<ProductUnit>>();
            foreach (var unit in productUnits)
                unit.ProductId = product.Id;


            //var inventory = new Inventory
            //{
            //    ProductId = product.Id,
            //    Quantity = request.Quantity
            //};
            if (request.Images is not null || request.Images.Any())
            {
                List<byte[]> images = new();

                foreach (var item in request.Images)
                {
                    var convert = Convert.FromBase64String(item);
                    images.Add(convert);
                }


                var uploadImages = await _imageService.UploadImageAsync(images, nameof(product));
                var newImages = new List<ProductImage>();
                foreach (var image in uploadImages)
                {
                    newImages.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        PublicId = image.PublicId,
                        ImageUrl = image.ImageUrl,
                        ThumbnailUrl = image.ThumbnailUrl,
                    });
                }
                await _context.ProductImages.AddRangeAsync(newImages);

            }

            _context.Products.Update(product);
            await _context.ProductUnits.AddRangeAsync(productUnits);
            //await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Result.Success(product.Id);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
    public async Task<Result> UpdateProductAsync(int id, UpdateProductRequest request)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var productExisting = await _context.Products.AnyAsync(p =>
                                                    p.Name == request.Name
                                                    && p.BrandId == request.BrandId
                                                    && p.CategoryId == request.CategoryId
                                                     && p.Id != id);

            if (productExisting)
                return Result.Failure(ProductErrors.ProductAlreadyExists);

            var product = await _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Images)
                .Include(p => p.Inventory)
                .Include(p => p.productUnits)
                .FirstOrDefaultAsync();

            if (product is null)
                return Result.Failure(ProductErrors.ProductNotFound);

            var productUpdate = request.Adapt(product);

            if (request.Images != null && request.Images.Any())
            {

                if (product.Images != null)
                {
                    foreach (var existingImage in product.Images)
                    {
                        await _imageService.DeleteImageAsync(existingImage.PublicId);
                        _context.ProductImages.Remove(existingImage);
                    }

                }

                List<byte[]> images = new();

                foreach (var item in request.Images)
                {
                    var convert = Convert.FromBase64String(item);
                    images.Add(convert);
                }


                var upload = await _imageService.UploadImageAsync(images, nameof(product));
                var newImages = new List<ProductImage>();
                foreach (var item in upload)
                {
                    newImages.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        PublicId = item.PublicId,
                        ImageUrl = item.ImageUrl,
                        ThumbnailUrl = item.ThumbnailUrl!,
                    });
                }
                await _context.ProductImages.AddRangeAsync(newImages);

            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Result.Success();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
    //public async Task<Result<byte[]>> GetBarcodeAsync(int productId)
    //{
    //    if (await _context.Products.FindAsync(productId) is not { } product)
    //        return Result.Failure<byte[]>(ProductErrors.ProductNotFound);

    //    var barcodePath = await _barcodeService.GenerateBarcodeAsync(product.Barcode!);

    //    return Result.Success(barcodePath);
    //}
    public async Task<Result<BarcodeResponse>> GetBarcodeAsync(int productId)
    {
        var product = await _context.Products
            .Include(p => p.productUnits)
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return Result.Failure<BarcodeResponse>(ProductErrors.ProductNotFound);

        // توليد الباركود مع الكمية
        var barcodePath = await _barcodeService.GenerateBarcodeAsync(product.Barcode!);
        var response = new BarcodeResponse
        {
            BarcodeImage = barcodePath,
            ProductCode = product.Barcode!,
            ProductName = product.Name!,
            ProductPrice = product.productUnits.FirstOrDefault(u => u.IsBaseUnit)?.SellingPrice ?? 0

        };

        return Result.Success(response);
    }
    //public async Task<Result<bool>> PrintBarcode(int productId, int quantity)
    //{
    //    if (await _context.Products.FindAsync(productId) is not { } product)
    //        return Result.Failure<bool>(ProductErrors.ProductNotFound);

    //    // توليد الباركود مع الكمية
    //    var barcodePath = await _barcodeService.PrintBarcodeAsync(product.Barcode!, quantity);

    //    return  Result.Success(barcodePath);
    //}

    public async Task<Result<ProductByBarcodeDto>> GetProductByBarcodeAsync(string barcode)
    {
        var product = _context.Products
                         .Where(p => p.Barcode == barcode && !p.IsDeleted)
                         .Include(p => p.productUnits)
                         .ThenInclude(u => u.Unit)
                         .FirstOrDefault();

        if (product is null)
            return Result.Failure<ProductByBarcodeDto>(ProductErrors.ProductNotFound);

        var baseUnit = product.productUnits.FirstOrDefault(u => u.IsBaseUnit);

        if (baseUnit == null)
            return Result.Failure<ProductByBarcodeDto>(ProductErrors.UnitNotFound);


        var dto = new ProductByBarcodeDto
        {
            ProductId = product.Id,
            ProductName = product.Name,
            BaseUnitId = baseUnit.UnitId,
            BaseUnitPrice = baseUnit.SellingPrice,
            BaseUnitName = baseUnit.Unit!.Name,
            Status = product.Status.ToString(),
        };


        return Result.Success(dto);

    }
    public async Task<Result<ProductByBarcodeForInvoiceSalesDto>> GetProductByBarcodeForInvoiceSalesAsync(string barcode)
    {
        var product = _context.Products
                         .Where(p => p.Barcode == barcode && !p.IsDeleted)
                         .Include(p => p.productUnits)
                         .ThenInclude(u => u.Unit)
                         .Include(p => p.Inventory)
                         .FirstOrDefault();

        if (product is null)
            return Result.Failure<ProductByBarcodeForInvoiceSalesDto>(ProductErrors.ProductNotFound);

        var baseUnit = product.productUnits.ToList();

        if (baseUnit == null)
            return Result.Failure<ProductByBarcodeForInvoiceSalesDto>(ProductErrors.UnitNotFound);


        var dto = new ProductByBarcodeForInvoiceSalesDto
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Units = baseUnit.Select(x => new ProductUnitDto
            {
                Id = x.UnitId,
                Name = x.Unit!.Name,
                NumberOfUnits = x.QuantityInBaseUnit,
                Price = x.SellingPrice,
                IsBaseUnit = x.IsBaseUnit,
            }).ToList(),
            Status = product.Status.ToString(),
            AvailableStock = product.Inventory?.Quantity ?? 0,
            Discount = product.Discount?.Value ?? 0
            //FinalPrice = product.Discount is not null
            //    ? (product.Discount.IsPercentage == true
            //        ? product.PurchasePrice - (product.PurchasePrice * (product.Discount.Value) / 100)
            //        : product.PurchasePrice - (product.Discount.Value ))
            //    : product.PurchasePrice
        };

        return Result.Success(dto);

    }
    //public async Task<Result<ProductResponseDetails>> GetProductByBarcodeAsync(string barcode)
    //{


    //    var product = await (from p in _context.Products
    //                         .AsNoTracking()
    //                         where p.Barcode == barcode
    //                         select new ProductResponseDetails
    //                         {
    //                             Id = p.Id,
    //                             Name = p.Name,
    //                             PurchasePrice = p.PurchasePrice,
    //                             Description = p.Description,
    //                             Quantity = p.Inventory!.Quantity,
    //                             Images = p.Images!.Select(i => new ImageRequest(i.ImageUrl, i.ThumbnailUrl)).ToList(),
    //                             Discount = new DiscountDto(p.Discount.Name ?? "No Discount", p.Discount.Value, p.Discount.IsPercentage),
    //                             ProductUnit = p.productUnits.Select(u => new ProductUnitDto
    //                             {
    //                                 Id = u.UnitId,
    //                                 Name = u.Unit!.Name,
    //                                 NumberOfUnits = u.QuantityInBaseUnit,
    //                                 Price = u.SellingPrice
    //                             }).ToList()
    //                         }).SingleOrDefaultAsync();

    //    if (product is null)
    //        return Result.Failure<ProductResponseDetails>(ProductErrors.ProductNotFound);

    //    return Result.Success(product);

    //}
    public async Task<Result<ProductResponseDetails>> GetProductDetailsAsync(int id)
    {

        var product = await (from p in _context.Products
                                 .AsNoTracking()
                             where p.Id == id
                             select new ProductResponseDetails
                             {
                                 Id = p.Id,
                                 Name = p.Name ?? "Unnamed Product",
                                 PurchasePrice = p.PurchasePrice,
                                 Description = p.Description ?? "No description available",
                                 Quantity = p.Inventory!.Quantity,
                                 Images = p.Images!.Select(i => new ImageRequest(i.ImageUrl, i.ThumbnailUrl)).ToList(),
                                 Discount = new DiscountDto { Name = p.Discount.Name ?? "No Discount", Value = p.Discount.Value, IsPercentage = p.Discount.IsPercentage },
                                 ProductUnit = p.productUnits.Select(u => new ProductUnitDto
                                 {
                                     Id = u.UnitId,
                                     Name = u.Unit!.Name,
                                     NumberOfUnits = u.QuantityInBaseUnit,
                                     Price = u.SellingPrice
                                 }).ToList()

                             }).SingleOrDefaultAsync();

        if (product is null)
            return Result.Failure<ProductResponseDetails>(ProductErrors.ProductNotFound);

        return Result.Success(product);

    }
    public async Task<Result<ProductResponseForUpdate>> GetProductForUpdateAsync(int id)
    {

        var product = await (from p in _context.Products
                                 .AsNoTracking()
                             where p.Id == id
                             select new ProductResponseForUpdate
                             {
                                 Name = p.Name,
                                 Description = p.Description,
                                 Images = p.Images.Select(x => new ImageResponse
                                 {
                                     ImageUrL = x.ImageUrl ?? "",
                                     ThumbnailUrl = x.ThumbnailUrl ?? ""
                                 }).ToList(),
                                 DiscountId = p.DiscountId,
                                 BrandId = p.BrandId,
                                 CategoryId = p.CategoryId


                             }).SingleOrDefaultAsync();

        if (product is null)
            return Result.Failure<ProductResponseForUpdate>(ProductErrors.ProductNotFound);

        return Result.Success(product);

    }

    public async Task<Result<PaginatedListResponse<ProductResponse>>> GetAll(RequestFilters filters)
    {
        var products = _context.Products
            .AsNoTracking()
            .Include(x => x.Images)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Quantity = p.Inventory != null ? p.Inventory.Quantity : 0,
                ProductUnits = p.productUnits
                    .Where(x => x.IsBaseUnit)
                    .Select(u => new ProductUnitDto
                    {
                        Id = u.UnitId,
                        Name = u.Unit.Name ?? "Unknown",
                        NumberOfUnits = u.QuantityInBaseUnit,
                        Price = u.SellingPrice
                    }).FirstOrDefault() ?? new ProductUnitDto(),
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                Images = p.Images.Select(x => new ImageResponse
                {
                    ImageUrL = x.ImageUrl ?? "",
                    ThumbnailUrl = x.ThumbnailUrl ?? ""
                }).ToList()
            });

        if (!string.IsNullOrEmpty(filters.SearchValue))
        {
            var searchValue = filters.SearchValue.ToLower();
            products = products.Where(x =>
                x.Name.ToLower().Contains(searchValue));
        }

        var response = await PaginatedList<ProductResponse>.CreatePaginationAsync(products, filters.PageNumber, filters.PageSize);

        if (response is null)
            return Result.Failure<PaginatedListResponse<ProductResponse>>(ProductErrors.ProductNotFound);

        var dto = response.Adapt<PaginatedListResponse<ProductResponse>>();

        return Result.Success(dto);
    }



}
