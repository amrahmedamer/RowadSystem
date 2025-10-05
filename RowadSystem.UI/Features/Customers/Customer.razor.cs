using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Phones;
using RowadSystem.Shard.Contract.Suppliers;
using RowadSystem.UI.Features.Addresses;

namespace RowadSystem.UI.Features.Customers;

public partial class Customer
{
    [Inject]
    ICustomerService _customerService { get; set; } = default!;
    [Inject]
    private IAddressService _addressService { get; set; } = default!;
    private CustomerRequest _customerRequest = new CustomerRequest()
    {
        Address = new AddressRequest(),
        PhoneNumbers = new List<PhoneNumberRequest>()
    };
    public PaginatedListResponse<CustomerResponse>  CustomerList { get; set; } = new();

    private List<SupplierResponse> suppliers = new();
    public RequestFilters filters { get; set; } = new();
    private int CurrentPage = 1;
    private int PageSize = 10;
    private bool IsLoading = true;

    private Modal modal;
    private List<GovernrateResponse> _governrateResponse;
    private List<CitiesResponse> _citiesResponse = new();

    protected override async Task OnInitializedAsync()
    {
        _governrateResponse = await _addressService.GetGovernratesAsync();
        await LoadCustmerAsync();
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
    private void OpenModal()
    {
        modal.Show();
    }
    private void CloseModal()
    {
        modal.Hide();
    }
   
    private async Task HandleSubmit()
    {
        var response = await _customerService.AddCustomer(_customerRequest);
        if (response.IsSuccessStatusCode)
        {
            CloseModal();
            await LoadCustmerAsync();
            StateHasChanged();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("❌ Error: " + error);
        }

    }

    private void AddPhoneNumber()
    {

        _customerRequest.PhoneNumbers?.Add(new PhoneNumberRequest());
    }

    private void RemovePhoneNumber(int index)
    {
        if (_customerRequest.PhoneNumbers.Count > 1)
        {
            _customerRequest.PhoneNumbers.RemoveAt(index);
        }
    }

    private void SetAsPrimary(int index)
    {
        for (int i = 0; i < _customerRequest.PhoneNumbers.Count; i++)
        {
            _customerRequest.PhoneNumbers[i].IsPrimary = (i == index);
        }
    }



   
    private async Task LoadCustmerAsync()
    {
        IsLoading = true;

        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;

        CustomerList = await _customerService.GetAllCustomer(filters);

        IsLoading = false;


    }
    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            PageSize = size;
            CurrentPage = 1;
            await LoadCustmerAsync();
        }
    }

    private async Task ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= CustomerList.TotalPages)
        {
            CurrentPage = pageNumber;
            await LoadCustmerAsync();
        }
    }

}
