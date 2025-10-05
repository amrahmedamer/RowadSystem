using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.Invoices;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Units;
using RowadSystem.UI.Features.Customers;
using System.Text.Json;

namespace RowadSystem.UI.Features.Invoices.SalesReturn;

public partial class Create
{

    [Inject]
    private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject]
    private AlertService AlertService { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    private ICustomerService _customerService { get; set; } = default!;
    private string _invoiceNumber { get; set; } = null!;
    private string _barcode { get; set; } = null!;

    private List<UnitResponse> _unitResponses = new();
    private CustomerResponseDto _customerResponse = new();
    //private InvoiceResponse _invoiceResponse = new();
    private SalesInvoiceResponse _invoiceResponse = new();
    private bool isLoading = false;
    private bool isSaving = false;
    private string errorMessage;

    private InvoiceItemResponse _invoiceItemResponse = new();
    private SalesReturnItemRequest _salesReturnItemRequest = new();
    private SalesReturnInvoiceRequest invoiceReturn = new()
    {
        Payments = new List<PaymentRequest>(),
        Items = new List<SalesReturnItemRequest>()
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
            var result = await InvoiceService.GetSalesInvoiceByInvoiceNumberAsync(_invoiceNumber);
            if (result.IsSuccess)
            {
                var customers = await _customerService.GetCustomersAsync();
                _customerResponse = customers.Value.FirstOrDefault(x => x.Id == result.Value.CustomerId)!;
                _invoiceResponse = result.Value;
                invoiceReturn.SalesInvoiceId = result.Value.SalesInvoiceId;
                invoiceReturn.CustomerId = _customerResponse.Id;
                invoiceReturn.UserId = null;

                errorMessage = null;
            }
            else
            {
                _customerResponse = new();
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
        //invoiceReturn.Barcode = _barcode;
        if (string.IsNullOrEmpty(_barcode))
        {
            //_invoiceItemResponse = null;
            StateHasChanged();
            isLoading = false;
            return;
        }

        var result = _invoiceResponse.Items.FirstOrDefault(x => x.Barcode == _barcode);
        if (result != null)
        {
            //Console.WriteLine(result.ProductName.ToString());
            _invoiceItemResponse = result;
            var unit = await InvoiceService.GetUnitsAsync();
            _unitResponses = unit.Value.Where(x => x.Id == result.UnitId).ToList();

            _salesReturnItemRequest = new SalesReturnItemRequest
            {

                Quantity = result.Quantity,
                ProductId = result.ProductId,
                UnitId = result.UnitId,
                ProductName = result.ProductName,
                UnitName = result.UnitName,

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
        if (_salesReturnItemRequest != null && _salesReturnItemRequest.Quantity > 0)
        {
            var selectedUnit = _unitResponses.FirstOrDefault(unit => unit.Id == _salesReturnItemRequest.UnitId);
            invoiceReturn.Items.Add(new SalesReturnItemRequest
            {
                ProductName = _salesReturnItemRequest.ProductName,
                UnitName = selectedUnit.Name,
                ProductId = _salesReturnItemRequest.ProductId,
                Quantity = _salesReturnItemRequest.Quantity,
                UnitId = _salesReturnItemRequest.UnitId
            });
            _salesReturnItemRequest = new SalesReturnItemRequest();
            _invoiceItemResponse = new InvoiceItemResponse();
            _barcode = null;
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

    private void RemoveItem(SalesReturnItemRequest item)
    {
        invoiceReturn.Items.Remove(item);
    }

    private async Task HandleValidSubmit()
    {
        isSaving = true;
        var json = JsonSerializer.Serialize(invoiceReturn);
        Console.WriteLine(json);
        var result = await InvoiceService.SalesInvoiceReturnAsync(invoiceReturn);

        if (result.IsSuccess)
        {
            AlertService.ShowSuccessAlert("Invoice successfully returned");

            invoiceReturn = new SalesReturnInvoiceRequest
            {
                Payments = new List<PaymentRequest>(),
                Items = new List<SalesReturnItemRequest>()
            };

            _customerResponse = new();
            _barcode = null;
            _invoiceNumber = null;
            NavigationManager.NavigateTo("/sales-invoice-return");
        }
        else
        {
            // Show error alert
            AlertService.ShowErrorAlert(result.Error.description);
        }


        isSaving = false;
    }
}
