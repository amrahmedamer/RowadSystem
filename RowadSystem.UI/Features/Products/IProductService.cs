
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Barcode;
using RowadSystem.Shard.Contract.Brands;
using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Discounts;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Products;
using RowadSystem.Shard.Contract.Suppliers;
using RowadSystem.Shard.Contract.Units;

namespace RowadSystem.UI.Features.Products;

public interface IProductService
{
    Task<PaginatedListResponse<ProductResponse>> GetProductsAsync(RequestFilters filters);
    Task<Result<int>> AddProductAsync(ProductRequest request);
    Task<List<CategoryResponse>> GetCategoriesAsync();
    Task<List<DiscountResponse>> GetAllDiscounts();
    Task<List<UnitResponse>> GetUnitsAsync();
    Task<List<BrandResponse>> GetBrandsAsync();
    Task<List<SupplierResponseDto>> GetSupplersAsync();
    //Task<Result<ProductResponseDetails>> GetProductByIdAsync(int id);
    Task<Result<ProductResponseDetails>> GetProductDetailsAsync(int id);
    Task<Result<ProductResponseForUpdate>> GetProductForUpdateAsync(int id);
    Task<Result> UpdateProductAsync(int id, UpdateProductRequest request);
    Task<BarcodeResponse> GetBarcodeAsync(int productId);
    //Task<byte[]> GetBarcodeAsync(int productId,int quantity);

}
