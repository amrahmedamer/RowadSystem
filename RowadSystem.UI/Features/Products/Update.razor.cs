using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RowadSystem.Shard.Contract.Brands;
using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Discounts;
using RowadSystem.Shard.Contract.Products;

namespace RowadSystem.UI.Features.Products;

public partial class Update
{
    private UpdateProductRequest updateProductRequest = new UpdateProductRequest();

    [Parameter] public int ProductId { get; set; }
    [Inject] public IProductService _productService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    private List<BrandResponse> _brandResponses { get; set; } = [];
    private List<CategoryResponse> _categoryResponses { get; set; } = [];
    private List<DiscountResponse> _discountResponses { get; set; } = [];
    private ProductResponseForUpdate? productDetails;
    private string ErrorMessage;
    protected override async Task OnInitializedAsync()
    {
        var result = await _productService.GetProductForUpdateAsync(ProductId);
        if (result.IsSuccess)
        {
            productDetails = result.Value;
            updateProductRequest = new UpdateProductRequest
            {
                Name = productDetails.Name,
                Description = productDetails.Description,
                DiscountId = productDetails.DiscountId,
                BrandId = productDetails.BrandId,
                CategoryId = productDetails.CategoryId,
            };
        }
        else
        {
            // Handle error (e.g., show a message to the user)
        }
        _categoryResponses = await _productService.GetCategoriesAsync();
        _brandResponses = await _productService.GetBrandsAsync();
        _discountResponses = await _productService.GetAllDiscounts();
    }

    private List<string> imagePreviews = new();

    private async Task HandleSelectedImages(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {

            if (file.Size > 5 * 1024 * 1024)
            {
                ErrorMessage = "One or more files exceed the 5 MB limit. Please select smaller files.";
                return;
            }

            ErrorMessage = null;


            var buffer = new byte[file.Size];
            using var stream = file.OpenReadStream(5 * 1024 * 1024);
            await stream.ReadAsync(buffer);

            var base64 = Convert.ToBase64String(buffer);
            imagePreviews.Add($"data:{file.ContentType};base64,{base64}");
            updateProductRequest.Images.Add(base64);


        }
    }
    private void RemoveImage(int index)
    {
        if (index >= 0 && index < updateProductRequest.Images.Count)
        {
            updateProductRequest.Images.RemoveAt(index);
            imagePreviews.RemoveAt(index);

        }
    }

    private async Task HandleValidSubmit()
    {
        var response = await _productService.UpdateProductAsync(ProductId, updateProductRequest);

        if (response.IsSuccess)
        {
            NavigationManager.NavigateTo($"/product-details/{ProductId}");
        }
        else
        {
            Console.WriteLine("Failed to update product.");
        }
    }
}


