using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Barcode;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Products;

namespace RowadSystem.Services;

public interface IProductService
{
    Task<Result<int>> AddProductAsync(string createByUserId, ProductRequest request);
    Task<Result> UpdateProductAsync(int id, UpdateProductRequest request);
    //Task<Result> UpdateProductAsync(int id, UpdateProductRequest request, List<IFormFile> images);
    //Task<Result<byte[]>> GetBarcodeAsync(int productId);
    Task<Result<BarcodeResponse>> GetBarcodeAsync(int productId);
    //Task<Result<bool>> PrintBarcode(int productId, int quantity);
    //Task<Result<ProductResponseDetails>> GetProductByBarcodeAsync(string barcode);
    Task<Result<ProductByBarcodeDto>> GetProductByBarcodeAsync(string barcode);
    Task<Result<ProductByBarcodeForInvoiceSalesDto>> GetProductByBarcodeForInvoiceSalesAsync(string barcode);
    //Task<Result<ProductResponseDetails>> GetProductByIdAsync(int id);
    Task<Result<PaginatedListResponse<ProductResponse>>> GetAll(RequestFilters filters);
    Task<Result<ProductResponseDetails>> GetProductDetailsAsync(int id);
    Task<Result<ProductResponseForUpdate>> GetProductForUpdateAsync(int id);

}
