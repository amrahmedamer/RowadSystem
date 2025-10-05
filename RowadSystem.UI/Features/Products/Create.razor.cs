using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using RowadSystem.Shard.Contract.Brands;
using RowadSystem.Shard.Contract.Categories;
using RowadSystem.Shard.Contract.Products;
using RowadSystem.Shard.Contract.Suppliers;
using RowadSystem.Shard.Contract.Units;


namespace RowadSystem.UI.Features.Products;

public partial class Create : ComponentBase
{
    [Inject]
    private IProductService _productService { get; set; } = default!;
    [Inject]
    NavigationManager _navigationManager { get; set; } = default!;
    [Inject]
    private IJSRuntime _jsruntime { get; set; } = default!;
    private ProductRequest ProductRequest { get; set; } = new();
    private List<CategoryResponse> _categoryResponses { get; set; } = [];
    private List<UnitResponse> _unitResponses { get; set; } = [];
    private List<BrandResponse> _brandResponses { get; set; } = [];
    //private List<SupplierResponseDto> _supplierResponses { get; set; } = [];
    private string ErrorMessage;

    protected override async Task OnInitializedAsync()
    {
        if (ProductRequest.UnitsWithConversion == null || !ProductRequest.UnitsWithConversion.Any())
            ProductRequest.UnitsWithConversion.Add(new UnitWithConversionDto());

        _categoryResponses = await _productService.GetCategoriesAsync();
        _unitResponses = await _productService.GetUnitsAsync();
        _brandResponses = await _productService.GetBrandsAsync();
        //_supplierResponses = await _productService.GetSupplersAsync();

    }
    void AddUnit()
    {
        ProductRequest.UnitsWithConversion.Add(new UnitWithConversionDto());
    }

    void RemoveUnit(int index)
    {

        if (index >= 0 && index < ProductRequest.UnitsWithConversion.Count)
        {
            ProductRequest.UnitsWithConversion.RemoveAt(index);
            InvokeAsync(StateHasChanged);
        }
        else
        {
            Console.WriteLine("❌ Index خارج النطاق!");
        }
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
            ProductRequest.Images.Add(base64);


        }
    }

    private void RemoveImage(int index)
    {
        if (index >= 0 && index < ProductRequest.Images.Count)
        {
            ProductRequest.Images.RemoveAt(index);
            imagePreviews.RemoveAt(index);
            Console.WriteLine($"Selected image count: {ProductRequest.Images.Count}");

        }
    }

    private async Task HandleValidSubmit()
    {
        var result = await _productService.AddProductAsync(ProductRequest);

        if (result.IsFailure)
        {
            ShowErrorAlert(result.Error.description);
        }
        else
        {
            ShowSuccessAlert("Product added successfully!");
            _navigationManager.NavigateTo("/products");
            StateHasChanged();

        }
    }
    private void ShowErrorAlert(string description)
    {
        _jsruntime.InvokeVoidAsync("showSweetAlert", "Error", description, "error", "Try Again");
        //await _jsruntime.InvokeVoidAsync("showSweetAlert", "Error", "Something went wrong!", "error", "Try Again");
    }

    private void ShowSuccessAlert(string description)
    {
        _jsruntime.InvokeVoidAsync("showSweetAlert", "Success", description, "success", "OK");
    }

}
