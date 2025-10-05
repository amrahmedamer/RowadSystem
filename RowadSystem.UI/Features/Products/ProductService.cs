
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
using System.Net.Http.Json;
using System.Text.Json;

namespace RowadSystem.UI.Features.Products;

public class ProductService(IHttpClientFactory httpClient) : IProductService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");

    public async Task<PaginatedListResponse<ProductResponse>> GetProductsAsync(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/Products?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}&searchValue={filters.SearchValue}");

        var products = await response.Content.ReadFromJsonAsync<PaginatedListResponse<ProductResponse>>();
        return products ?? new PaginatedListResponse<ProductResponse>();
    }
    //public async Task<byte[]> GetBarcodeAsync(int productId)
    //{
    //    var response = await _httpClient.GetAsync($"/api/Products/{productId}/barcode");
    //    response.EnsureSuccessStatusCode();
    //    return await response.Content.ReadAsByteArrayAsync();
    //}
    public async Task<BarcodeResponse> GetBarcodeAsync(int productId)
    {
        var response = await _httpClient.GetAsync($"/api/Products/{productId}/barcode");
        var result = await response.Content.ReadFromJsonAsync<BarcodeResponse>();
        return result ?? new BarcodeResponse();
    }
    //public async Task<byte[]> GetBarcodeAsync(int productId,int quantity)
    //{
    //    var response = await _httpClient.GetAsync($"/api/Products/{productId}/barcode/{quantity}");
    //    response.EnsureSuccessStatusCode();
    //    return await response.Content.ReadAsByteArrayAsync();
    //}

    public async Task<Result<int>> AddProductAsync(ProductRequest request)
    {
        Console.WriteLine(JsonSerializer.Serialize(request));
        var response = await _httpClient.PostAsJsonAsync("/api/Products", request);
        if (response.IsSuccessStatusCode)
        {
            var ProductId = await response.Content.ReadFromJsonAsync<int>();
            return Result.Success(ProductId);
        }
        else
        {

            await PorblemDetailsExtensions.HandleErrorResponse<int>(response);
        }


        return Result.Failure<int>(new Error("UnknownError", "An unknown error occurred.", 500));

    }
    public async Task<Result> UpdateProductAsync(int productId, UpdateProductRequest request)
    {
        //Console.WriteLine(JsonSerializer.Serialize(request));
        var response = await _httpClient.PutAsJsonAsync($"/api/Products/{productId}", request);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }
        else
        {

            await PorblemDetailsExtensions.HandleErrorResponse<int>(response);
        }


        return Result.Failure(new Error("UnknownError", "An unknown error occurred.", 500));

    }

    public async Task<Result<ProductResponseDetails>> GetProductDetailsAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/Products/{id}/details");
        if (response.IsSuccessStatusCode)
        {
            var Product = await response.Content.ReadFromJsonAsync<ProductResponseDetails>();
            return Result.Success(Product);
        }
        else
        {

            await PorblemDetailsExtensions.HandleErrorResponse<ProductResponseDetails>(response);
        }
        return Result.Failure<ProductResponseDetails>(new Error("UnknownError", "An unknown error occurred.", 500));

    }
    public async Task<Result<ProductResponseForUpdate>> GetProductForUpdateAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/Products/{id}/update");
        if (response.IsSuccessStatusCode)
        {
            var Product = await response.Content.ReadFromJsonAsync<ProductResponseForUpdate>();
            return Result.Success(Product);
        }
        else
        {

            await PorblemDetailsExtensions.HandleErrorResponse<ProductResponseForUpdate>(response);
        }
        return Result.Failure<ProductResponseForUpdate>(new Error("UnknownError", "An unknown error occurred.", 500));

    }
    public async Task<List<DiscountResponse>> GetAllDiscounts()
    {
        var response = await _httpClient.GetAsync("/api/Discounts");
        var discounts = await response.Content.ReadFromJsonAsync<List<DiscountResponse>>();
        return discounts ?? new List<DiscountResponse>();

    }
    public async Task<List<CategoryResponse>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("/api/Categories/without-filter");
        var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();
        return categories ?? new List<CategoryResponse>();

    }
    public async Task<List<UnitResponse>> GetUnitsAsync()
    {
        var response = await _httpClient.GetAsync("/api/Units");
        var categories = await response.Content.ReadFromJsonAsync<List<UnitResponse>>();
        return categories ?? new List<UnitResponse>();

    }
    public async Task<List<BrandResponse>> GetBrandsAsync()
    {
        var response = await _httpClient.GetAsync("/api/Brands");
        var brands = await response.Content.ReadFromJsonAsync<List<BrandResponse>>();
        return brands ?? new List<BrandResponse>();

    }
    public async Task<List<SupplierResponseDto>> GetSupplersAsync()
    {
        var response = await _httpClient.GetAsync("/api/Suppliers/without-filter");
        var supplier = await response.Content.ReadFromJsonAsync<List<SupplierResponseDto>>();
        return supplier ?? new List<SupplierResponseDto>();

    }


}

