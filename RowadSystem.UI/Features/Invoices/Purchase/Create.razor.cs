using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Invoices;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Products;
using RowadSystem.Shard.Contract.Suppliers;
using RowadSystem.UI.Features.Suppliers;

namespace RowadSystem.UI.Features.Invoices.Purchase;

public partial class Create
{


    [Inject]
    private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject]
    private AlertService AlertService { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    private ISupplierService  _supplierService { get; set; } = default!;
 
    private string _barcode { get; set; } = null!;
    //private PurchaseInvoiceItemRequest _purchaseInvoiceItems = new();
    //private List<ProductUnitDto> _unitResponses = new();
    //private ProductResponseDetails _products = new();
    private List<SupplierResponse> _supplierResponses = new();
    private ProductByBarcodeDto _productByBarcode = null;
    private PurchaseInvoiceDto _purchaseInvoiceDto = new();

    private bool isLoading = false;
    private bool isSaving = false;
    private PurchaseInvoiceRequest invoice = new()
    {
        Payments = new List<PaymentRequest>(),
        Items = new List<PurchaseInvoiceItemRequest>()
    };

    protected override async Task OnInitializedAsync()
    {

        var result = await _supplierService.GetSuppliersAsync();
        if (result.IsSuccess)
            _supplierResponses = result.Value;
        else
            AlertService.ShowErrorAlert(result.Error.description);

        invoice.Payments.Add(new PaymentRequest
        {
            Method = PaymentMethod.Cash,
            Amount = 0
        });


    }
    private async Task SearchProductByBarcode(ChangeEventArgs e)
    {
        isLoading = true;
        _barcode = e.Value?.ToString();
        if (string.IsNullOrEmpty(_barcode))
        {
            _productByBarcode = null;
            StateHasChanged();
            return;
        }

        var result = await InvoiceService.GetProductBybarcodeAsync(_barcode);
        if (result != null)
        {
            _productByBarcode = result.Value;
            StateHasChanged();
        }
        else
        {
            AlertService.ShowErrorAlert(result.Error.description);
        }
        isLoading = false;
    }
    private void AddItem()
    {
        if (_productByBarcode != null && _purchaseInvoiceDto.Quantity > 0 && _purchaseInvoiceDto.Price > 0)
        {
            var existingItem = invoice.Items.FirstOrDefault(item => item.ProductId == _productByBarcode.ProductId && item.UnitId == _productByBarcode.BaseUnitId);

            if (existingItem != null)
            {
                existingItem.Quantity += _purchaseInvoiceDto.Quantity;
                existingItem.Price = _purchaseInvoiceDto.Price;
            }
            else
            {

                invoice.Items.Add(new PurchaseInvoiceItemRequest
                {
                    ProductName = _productByBarcode.ProductName,
                    ProductId = _productByBarcode.ProductId,
                    UnitId = _productByBarcode.BaseUnitId,
                    Price = _purchaseInvoiceDto.Price,
                    Quantity = _purchaseInvoiceDto.Quantity,
                    UnitName = _productByBarcode.BaseUnitName

                });
            }

            _purchaseInvoiceDto = new PurchaseInvoiceDto();
            _productByBarcode = null;
            _barcode = null;
        }
        else
        {
            AlertService.ShowErrorAlert("Invalid Request");
        }
    }


    //private void AddItem()
    //{
    //    if (_productByBarcode != null && _purchaseInvoiceDto.Quantity > 0 && _purchaseInvoiceDto.Price > 0)
    //    {
    //        invoice.Items.Add(new PurchaseInvoiceItemRequest
    //        {
    //            ProductName = _productByBarcode.ProductName,
    //            ProductId = _productByBarcode.ProductId,
    //            UnitId = _productByBarcode.BaseUnitId,
    //            Price = _purchaseInvoiceDto.Price,
    //            Quantity = _purchaseInvoiceDto.Quantity,
    //            UnitName = _productByBarcode.BaseUnitName

    //        });
    //        //_purchaseInvoiceItems = new PurchaseInvoiceItemRequest();
    //        _productByBarcode = new ProductByBarcodeDto();
    //        _purchaseInvoiceDto = new PurchaseInvoiceDto();

    //    }
    //    else
    //    {
    //        AlertService.ShowErrorAlert("Invalid Request");
    //    }
    //}

    private void AddPayment()
    {

        if (invoice.Payments.Count < 3)
        {

            invoice.Payments.Add(new PaymentRequest
            {
                Method = PaymentMethod.Cash,
                Amount = 0
            });
        }
        else
        {
            AlertService.ShowErrorAlert("You can only add up to 3 payments.");
        }
    }
    private void RemovePayment(PaymentRequest payment)
    {
        // Remove payment from the list
        invoice.Payments.Remove(payment);
    }
    private void RemoveItem(PurchaseInvoiceItemRequest item)
    {
        invoice.Items.Remove(item);
    }

    private async Task HandleValidSubmit()
    {
        isSaving = true;
        var result = await InvoiceService.PurchaseInvoiceAsync(invoice);

        if (result.IsSuccess)
        {
            AlertService.ShowSuccessAlert("Invoice submitted successfully!");
            invoice = new PurchaseInvoiceRequest
            {
                Payments = new List<PaymentRequest>(),
                Items = new List<PurchaseInvoiceItemRequest>()
            };
            _barcode = null;
            NavigationManager.NavigateTo("/Purchase-invoice");
        }
        else
            AlertService.ShowErrorAlert(result.Error.description);
        isSaving = false;
    }
}


