using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Auth;
using RowadSystem.Shard.Contract.Phones;
using RowadSystem.UI.Features.Addresses;

namespace RowadSystem.UI.Features.Auth;

public partial class Register
{

    [Inject]
    private IAuthService _authService { get; set; } = default!;

    [Inject]
    private IAddressService _addressService { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private RegisterRequest _registerRequest = new();
    private string? _errorMessage;
    private List<GovernrateResponse> _governrateResponse;
    private List<CitiesResponse> _citiesResponse = new();

    protected override async Task OnInitializedAsync()
    {
        _governrateResponse = await _addressService.GetGovernratesAsync();
     
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
    private async Task HandleRegister()
    {
        try
        {
            await _authService.RegisterAsync(_registerRequest);
            NavigationManager.NavigateTo($"/Confirm-email?Email={Uri.EscapeDataString(_registerRequest.Email)}");
        }
        catch (ApplicationException ex)
        {
            _errorMessage = ex.Message;
        }
    }
 
    private void AddPhoneNumber()
    {
        if (_registerRequest.PhoneNumbers.Count < 2)
        {
            _registerRequest.PhoneNumbers.Add(new PhoneNumberRequest());
        }
    }

    private void RemovePhoneNumber(int index)
    {
        if (_registerRequest.PhoneNumbers.Count > 1)
        {
            _registerRequest.PhoneNumbers.RemoveAt(index);
        }
    }

    private void SetAsPrimary(int index)
    {
        for (int i = 0; i < _registerRequest.PhoneNumbers.Count; i++)
        {
            _registerRequest.PhoneNumbers[i].IsPrimary = (i == index);
        }
    }

}



