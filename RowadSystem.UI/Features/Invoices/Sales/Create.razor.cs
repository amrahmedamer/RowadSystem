using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.Invoices;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Products;
using RowadSystem.Shard.Contract.Units;
using RowadSystem.UI.Features.Customers;

namespace RowadSystem.UI.Features.Invoices.Sales;

public partial class Create
{

    [Inject]
    private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject]
    private AlertService AlertService { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    private ICustomerService  _customerService { get; set; } = default!;
    private string _barcode { get; set; } = null!;
    private SalesInvoiceItemRequest _salesInvoiceItems = new();
    private List<CustomerResponseDto> _customerResponses = new();
    private ProductByBarcodeForInvoiceSalesDto _productByBarcode = null;

    private bool isLoading = false;
    private bool isSaving = false;

    private string errorMessage;

    private SalesInvoiceRequest invoice = new()
    {
        Payments = new List<PaymentRequest>(),
        Items = new List<SalesInvoiceItemRequest>()
    };

    protected override async Task OnInitializedAsync()
    {
        var result = await _customerService.GetCustomersAsync();
        if (result.IsSuccess)
            _customerResponses = result.Value;
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

        var result = await InvoiceService.GetProductBybarcodeForInvoiceSalesAsync(_barcode);
        if (result != null)
        {
            _productByBarcode = new();
            _productByBarcode = result.Value;
            _salesInvoiceItems.UnitId = _productByBarcode.Units.FirstOrDefault(x => x.IsBaseUnit)!.Id;

            StateHasChanged();
        }
        else
        {
            AlertService.ShowErrorAlert(result.Error.description);
        }
        isLoading = false;
    }

    //private void AddItem()
    //{
    //    if (_productByBarcode != null && _salesInvoiceItems.UnitId != 0 && _salesInvoiceItems.Quantity > 0)
    //    {
    //        var selectedUnit = _productByBarcode.Units.FirstOrDefault(unit => unit.Id == _salesInvoiceItems.UnitId);
    //        if (selectedUnit != null)
    //        {
    //            invoice.Items.Add(new SalesInvoiceItemRequest
    //            {
    //                ProductId = _productByBarcode.ProductId,
    //                ProductName = _productByBarcode.ProductName,
    //                Quantity = _salesInvoiceItems.Quantity,
    //                UnitId = _salesInvoiceItems.UnitId,
    //                Price = selectedUnit.Price
    //            });
    //            _salesInvoiceItems = new();
    //            _productByBarcode = null;
    //            _barcode = null;

    //        }
    //    }
    //    else
    //    {
    //        AlertService.ShowErrorAlert("Invalid Request");
    //    }
    //}
    private void AddItem()
    {
        if (_productByBarcode != null && _salesInvoiceItems.UnitId != 0 && _salesInvoiceItems.Quantity > 0)
        {
            var selectedUnit = _productByBarcode.Units.FirstOrDefault(unit => unit.Id == _salesInvoiceItems.UnitId);
            if (selectedUnit != null)
            {
                var existingItem = invoice.Items.FirstOrDefault(item => item.ProductId == _productByBarcode.ProductId && item.UnitId == _salesInvoiceItems.UnitId);

                if (existingItem != null)
                {
                    existingItem.Quantity += _salesInvoiceItems.Quantity;
                }
                else
                {
                    invoice.Items.Add(new SalesInvoiceItemRequest
                    {
                        ProductId = _productByBarcode.ProductId,
                        ProductName = _productByBarcode.ProductName,
                        Quantity = _salesInvoiceItems.Quantity,
                        UnitId = _salesInvoiceItems.UnitId,
                        Price = selectedUnit.Price
                    });
                }

                _salesInvoiceItems = new SalesInvoiceItemRequest();
                _productByBarcode = null;
                _barcode = null;
            }
        }
        else
        {
            AlertService.ShowErrorAlert("Invalid Request");
        }
    }
   

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
        invoice.Payments.Remove(payment);
    }
    private void RemoveItem(SalesInvoiceItemRequest item)
    {
        invoice.Items.Remove(item);
    }

    private async Task HandleValidSubmit()
    {
        isSaving = true;

        var result = await InvoiceService.SalesInvoiceAsync(invoice);

        if (result.IsSuccess)
        {
            AlertService.ShowSuccessAlert("Invoice submitted successfully!");
            invoice = new SalesInvoiceRequest
            {
                Items = new List<SalesInvoiceItemRequest>(),
                Payments = new List<PaymentRequest>()
            };
            _barcode = null;
            NavigationManager.NavigateTo("/sales-invoice");
        }
        else
            AlertService.ShowErrorAlert(result.Error.description);

        isSaving = false;

    }
}


