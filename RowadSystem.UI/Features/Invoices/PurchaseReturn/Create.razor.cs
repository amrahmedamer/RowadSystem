using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Invoices;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Suppliers;
using RowadSystem.Shard.Contract.Units;
using System.Text.Json;


namespace RowadSystem.UI.Features.Invoices.PurchaseReturn;

public partial class Create
{



    [Inject]
    private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject]
    private AlertService AlertService { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    private string _invoiceNumber { get; set; } = null!;
    private string _barcode { get; set; } = null!;
    private int _returnQuantity { get; set; }

    private UnitResponse _unitResponses = new();
    private SupplierResponse _supplierResponse = new();
    //private InvoiceResponse _invoiceResponse = new();
    private PurchaseInvoiceResponse _purchaseInvoiceResponse = new();
    private bool isLoading = false;
    private bool isSaving = false;
    private string errorMessage;
    private InvoiceItemResponse _invoiceItemResponse = null;
    private PurchaseReturnInvoiceItemRequest _purchaseReturnItemRequest = new();
    private PurchaseReturnInvoiceRequest invoiceReturn = new()
    {
        Payments = new List<PaymentRequest>(),
        Items = new List<PurchaseReturnInvoiceItemRequest>()
    };


    private async Task SearchByInvoiceNumber(ChangeEventArgs e)
    {
        isLoading = true;
        _invoiceNumber = e.Value?.ToString();
        if (string.IsNullOrEmpty(_invoiceNumber))
        {
            _invoiceItemResponse = null;
            StateHasChanged();
            isLoading = false;
            return;
        }


        try
        {
            var result = await InvoiceService.GetPurchaseInvoiceByInvoiceNumberAsync(_invoiceNumber);
            if (result.IsSuccess)
            {
                //Console.WriteLine(JsonSerializer.Serialize<InvoiceResponse>(result.Value));
                //var suppliers = await InvoiceService.GetSuppliersAsync();
                //_supplierResponse = suppliers.Value.FirstOrDefault(x => x.Id == result.Value.SupplierId)!;
                //invoiceReturn.SupplierId = _supplierResponse.Id;
                _supplierResponse.Name = result.Value.SupplierName;
                invoiceReturn.SupplierId = result.Value.SupplierId;

                _purchaseInvoiceResponse = result.Value;
                invoiceReturn.PurchaseInvoiceId = result.Value.PurchaseInvoiceId;
                errorMessage = null;
            }
            else
            {
                errorMessage = result.Error.description;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        isLoading = false;
        StateHasChanged();
    }
    private async Task SearchProductByBarcode(ChangeEventArgs e)
    {
        isLoading = true;
        _barcode = e.Value?.ToString();
        if (string.IsNullOrEmpty(_barcode))
        {
            _invoiceItemResponse = null;
            StateHasChanged();
            isLoading = false;
            return;
        }

        var result = _purchaseInvoiceResponse.Items.FirstOrDefault(x => x.Barcode == _barcode);
        if (result != null)
        {

            _invoiceItemResponse = new();
            _invoiceItemResponse = result;
            ////Console.WriteLine($"Unit Name : {result.productUnits.Name}");
            //Console.WriteLine($"Unit Name : {result.UnitName}");

            //var unit = await InvoiceService.GetUnitsAsync();
            //_unitResponses = unit.Value.Where(x => x.Id == result.UnitId).ToList();

            _purchaseReturnItemRequest = new PurchaseReturnInvoiceItemRequest
            {
                Quantity = result.Quantity,
                ProductId = result.ProductId,
                UnitId = result.UnitId,
                ProductName = result.ProductName,
                UnitName = result.UnitName

            };
        }
        else
        {
            _invoiceItemResponse = null;
            errorMessage = "Product not found in the invoice";
        }
        isLoading = false;
        StateHasChanged();
    }


    private void AddItem()
    {
        if (_purchaseReturnItemRequest != null && _returnQuantity > 0)
        {
            //var selectedUnit = _unitResponses.FirstOrDefault(unit => unit.Id == _purchaseReturnItemRequest.UnitId);
            invoiceReturn.Items.Add(new PurchaseReturnInvoiceItemRequest
            {
                ProductId = _purchaseReturnItemRequest.ProductId,
                Quantity = _returnQuantity,
                UnitId = _purchaseReturnItemRequest.UnitId,
                UnitName = _purchaseReturnItemRequest.UnitName,
                ProductName = _purchaseReturnItemRequest.ProductName
            });
            _purchaseReturnItemRequest = new PurchaseReturnInvoiceItemRequest();
            _invoiceItemResponse = null;
            _barcode = null;
            _returnQuantity = default;

            StateHasChanged();

        }
        else
        {
            AlertService.ShowErrorAlert("Invalid item or quantity");
        }
    }

    private void AddPayment()
    {
        invoiceReturn.Payments.Add(new PaymentRequest { Method = PaymentMethod.Cash, Amount = 0 });
    }

    private void RemovePayment(PaymentRequest payment)
    {
        invoiceReturn.Payments.Remove(payment);
    }

    private void RemoveItem(PurchaseReturnInvoiceItemRequest item)
    {
        invoiceReturn.Items.Remove(item);
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            isSaving = true;
            Console.WriteLine(JsonSerializer.Serialize<PurchaseReturnInvoiceRequest>(invoiceReturn));
            var result = await InvoiceService.PurchaseInvoiceReturnAsync(invoiceReturn);

            if (result.IsSuccess)
            {
                AlertService.ShowSuccessAlert("Invoice successfully returned");
                invoiceReturn = new PurchaseReturnInvoiceRequest
                {
                    Payments = new List<PaymentRequest>(),
                    Items = new List<PurchaseReturnInvoiceItemRequest>()
                };
                _invoiceNumber = null;
                NavigationManager.NavigateTo("/purchase-invoice-return");
            }
            else
                AlertService.ShowErrorAlert(result.Error.description);

            isSaving = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing return: {ex.Message}");
        }
    }
}


