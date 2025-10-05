using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RowadSystem.Shard.Contract.Products;
using System.Threading.Tasks;

namespace RowadSystem.UI.Features.Products;

public partial class Details
{
    [Parameter] public int ProductId { get; set; }
    [Inject] public IProductService productService { get; set; } = default!;
    [Inject] public IJSRuntime   JSRuntime { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    private ProductResponseDetails? productDetails;
    private string barcodeUrl;

    protected override async Task OnInitializedAsync()
    {
        var result = await productService.GetProductDetailsAsync(ProductId);
        if (result.IsSuccess)
        {
            productDetails = result.Value;
        }
        else
        {
            // Handle error (e.g., show a message to the user)
        }
    }

    private void Back()
    {
        NavigationManager.NavigateTo("/products");
    }
    private async Task GetBarcode()
    {
        var barcode =await productService.GetBarcodeAsync(ProductId);
      barcodeUrl= $"data:image/png;base64,{Convert.ToBase64String(barcode.BarcodeImage)}";
        //NavigationManager.NavigateTo(url, true);
    }
    private async Task PrintBarcode()
    {
        NavigationManager.NavigateTo($"/print-barcode/{ProductId}", true);

    }
    //private async Task PrintBarcode()
    //{

    //    var barcode = await productService.GetBarcodeAsync(ProductId);
    //    var base64Barcode = Convert.ToBase64String(barcode);
    //    var url = $"data:image/png;base64,{base64Barcode}";

    //    // استدعاء دالة JavaScript للطباعة
    //    await JSRuntime.InvokeVoidAsync("printBarcode", url);
    //}

}

