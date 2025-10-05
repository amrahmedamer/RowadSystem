using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Orders;
using RowadSystem.Shard.Contract.Payments;
using RowadSystem.Shard.Contract.Phones;
using RowadSystem.UI.Features.Addresses;
using System.Text.Json;

namespace RowadSystem.UI.Features.Payments;

public partial class Address
{
    [Inject]
    private IAddressService _addressService { get; set; } = default!;
    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;
    [Inject] public IPaymentService PaymentService { get; set; }
    private List<GovernrateResponse> _governrateResponse;
    private List<CitiesResponse> _citiesResponse = new();
    private OrderRequest _orderRequest = new()
    {
        Address = new AddressRequest() { CityId = 0, GovernorateId = 0 },
        PhoneNumbers = new PhoneNumberRequest()
    };
    private CheckoutResponse CheckoutResponse = new();
    protected override async Task OnInitializedAsync()
    {
        _governrateResponse = await _addressService.GetGovernratesAsync();
    }
    private async Task ProcessCheckoutAsync()
    {
        var response = await PaymentService.ProcessToCheckOutAsync(_orderRequest);
        if (response.IsSuccess)
        {
            CheckoutResponse = response.Value;
            // عند التنقل:
            var checkoutJson = JsonSerializer.Serialize(CheckoutResponse);
            _navigationManager.NavigateTo($"checkout-payment?checkoutData={Uri.EscapeDataString(checkoutJson)}");

        }
        else
        {
            Console.WriteLine("Checkout failed.");
        }
    }
    private async Task OnGovernorateChanged(ChangeEventArgs e)
    {
        var governorateId = Convert.ToInt32(e.Value);
        if (governorateId > 0)
            await LoadCities(governorateId);
        else
            _citiesResponse.Clear();
    }
    private async Task LoadCities(int governorateId)
    {

        var cities = await _addressService.GetCitiesAsync();
        _citiesResponse = cities.Where(x => x.GovernorateId == governorateId).ToList();
    }

}
