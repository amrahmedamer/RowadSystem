//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;

//namespace RowadSystem.UI.Features.Products
//{
//    public partial class PrintBarcode
//    {
//        [Inject]
//        IProductService ProductService { get; set; } = null!; // تعريف الخدمة

//        [Inject]
//        IJSRuntime JSRuntime { get; set; } = null!; // تعريف الخدمة

//        [Parameter]
//        public int ProductId { get; set; } // معرف المنتج لطباعة الباركود له

//        private int quantity = 1; // الكمية المبدئية
//        private byte[] barcodeBytes;
//        private string base64Barcode = string.Empty;

//        private async Task PrintBarcodeAsync()
//        {
//            // استدعاء الخدمة للحصول على الباركود
//            barcodeBytes = await ProductService.GetBarcodeAsync(ProductId, quantity);

//            if (barcodeBytes != null && barcodeBytes.Length > 0)
//            {
//                // تحويل الباركود إلى Base64 لتتمكن من عرضه كصورة
//                base64Barcode = Convert.ToBase64String(barcodeBytes);

//                // التأكد من أن الـ base64Barcode يحتوي على قيمة صحيحة
//                if (string.IsNullOrEmpty(base64Barcode))
//                {
//                    Console.WriteLine("حدث خطأ في توليد الباركود");
//                    return;
//                }

//                // استدعاء JavaScript للطباعة
//                await JSRuntime.InvokeVoidAsync("printBarcode", base64Barcode);

//                Console.WriteLine("تمت الطباعة بنجاح");
//            }
//            else
//            {
//                Console.WriteLine("حدث خطأ في توليد الباركود");
//            }
//        }


//    }
//}


using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace RowadSystem.UI.Features.Products
{
    public partial class PrintBarcode
    {
        [Inject]
        IProductService ProductService { get; set; } = null!; // Inject the product service

        [Inject]
        IJSRuntime JSRuntime { get; set; } = null!; // Inject the JSRuntime service

        [Parameter]
        public int ProductId { get; set; } // Product ID to print the barcode for

        private string ProductName ; // Initial quantity
        private decimal ProductPrice ; // Initial quantity
        private byte[] barcodeBytes;
        private string base64Barcode = string.Empty;

        // Asynchronous method to get the barcode and generate the base64 string
        private async Task PrintBarcodeAsync()
        {
            // Fetch the barcode byte array from the service
            var result= await ProductService.GetBarcodeAsync(ProductId);
            barcodeBytes = result.BarcodeImage!;
            ProductName = result.ProductName;
            ProductPrice = result.ProductPrice;

            if (barcodeBytes != null && barcodeBytes.Length > 0)
            {
                // Convert the barcode to Base64 for easy image rendering
                base64Barcode = Convert.ToBase64String(barcodeBytes);

                // Ensure base64Barcode is not empty
                if (string.IsNullOrEmpty(base64Barcode))
                {
                    Console.WriteLine("Error generating barcode");
                    return;
                }
            }
            else
            {
                Console.WriteLine("No barcode data available.");
            }
        }

        // Method to print the barcode using JSInterop
        private async Task PrintPage()
        {
            if (!string.IsNullOrEmpty(base64Barcode))
            {
                await JSRuntime.InvokeVoidAsync("printBarcode", base64Barcode, ProductName, ProductPrice);
            }
            else
            {
                Console.WriteLine("Barcode is not ready to print.");
            }
        }
    }
}
