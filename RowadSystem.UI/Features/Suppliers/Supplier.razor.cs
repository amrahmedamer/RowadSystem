using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Phones;
using RowadSystem.Shard.Contract.Suppliers;
using RowadSystem.UI.Features.Addresses;

namespace RowadSystem.UI.Features.Suppliers;

public partial class Supplier
{

    [Inject]
    private ISupplierService _supplierService { get; set; } = default!;
    [Inject]
    private IAddressService _addressService { get; set; } = default!;
    private bool IsLoading = true;
    private List<SupplierResponse> _supplierResponses = new List<SupplierResponse>();
    private SupplierRequest _supplierRequest = new SupplierRequest()
    {
        Address = new AddressRequest(),
        PhoneNumbers = new List<PhoneNumberRequest>()
    };
    public PaginatedListResponse<SupplierResponse> SupplierList { get; set; } = new();
    public RequestFilters filters { get; set; } = new();
    private List<SupplierResponse> suppliers = new();
    private int CurrentPage = 1;
    private int PageSize = 10;
    private Modal modal = default!;
    private List<GovernrateResponse> _governrateResponse;
    private List<CitiesResponse> _citiesResponse = new();

    protected override async Task OnInitializedAsync()
    {
        _governrateResponse = await _addressService.GetGovernratesAsync();
        await LoadSuppliersAsync();

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
        var response = await _supplierService.AddSupplier(_supplierRequest);
        if (response.IsSuccessStatusCode)
        {
            CloseModal();
            await LoadSuppliersAsync();
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
        // Add a new phone number entry
        _supplierRequest.PhoneNumbers?.Add(new PhoneNumberRequest());
    }

    private void RemovePhoneNumber(int index)
    {
        if (_supplierRequest.PhoneNumbers.Count > 1)
        {
            _supplierRequest.PhoneNumbers.RemoveAt(index);
        }
    }

    private void SetAsPrimary(int index)
    {
        for (int i = 0; i < _supplierRequest.PhoneNumbers.Count; i++)
        {
            _supplierRequest.PhoneNumbers[i].IsPrimary = (i == index);
        }

    }


    private async Task LoadSuppliersAsync()
    {
        IsLoading = true;

        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;

        SupplierList = await _supplierService.GetAllSupplier(filters);

        IsLoading = false;


    }
    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            PageSize = size;
            CurrentPage = 1;
            await LoadSuppliersAsync();
        }
    }

    private async Task ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= SupplierList.TotalPages)
        {
            CurrentPage = pageNumber;
            await LoadSuppliersAsync();
        }
    }


}

