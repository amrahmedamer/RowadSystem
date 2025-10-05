using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Phones;
using RowadSystem.Shard.Contract.Roles;
using RowadSystem.Shard.Contract.Suppliers;
using RowadSystem.Shard.Contract.Users;
using RowadSystem.UI.Features.Addresses;

namespace RowadSystem.UI.Features.Users;

public partial class User
{
    [Inject]
    private IUserService _userService { get; set; } = default!;
    [Inject]
    private IAddressService _addressService { get; set; } = default!;
    [Inject]
    private IJSRuntime JS { get; set; } = default!;
    [Inject]
    private AlertService _alertService { get; set; } = default!;
    //private List<string> SelectedRoles { get; set; } = new();
    private UserRequest _userRequest = new UserRequest
    {
        Address = new AddressRequest(),
        PhoneNumbers = new List<PhoneNumberRequest>(),
        Roles = new()
    };

    private List<string> _rolesResponse { get; set; } = new();
    //private ElementReference roleSelect;
    List<RoleRespons> selectedRoles = new();


    public PaginatedListResponse<UserResponse> UserList { get; set; } = new();
    public RequestFilters filters { get; set; } = new();
    private bool IsLoading = true;
    private int CurrentPage = 1;
    private int PageSize = 10;
    private Modal modal = default!;
    private List<GovernrateResponse> _governrateResponse;
    private List<CitiesResponse> _citiesResponse = new();
    protected override async Task OnInitializedAsync()
    {
        _governrateResponse = await _addressService.GetGovernratesAsync();
        _rolesResponse = await _userService.GetAllRoles();
        await LoadUsersAsync();

    }
    // Initialize Select2 when the component renders
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Initialize Select2 on the select element
            await JS.InvokeVoidAsync("eval", "$('#searchSelect').select2();");

        }
    }


    private async Task OnGovernorateChanged(ChangeEventArgs e)
    {
        if (e.Value != null)
        {
            var governorateId = Convert.ToInt32(e.Value);
            if (governorateId > 0)
                await LoadCities(governorateId);
            else
                _citiesResponse.Clear();
        }
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
    private void HandleClose()
    {
        CloseModal();
    }

    private async Task HandleSubmit()
    {
        //var selectedRoles = await JS.InvokeAsync<string[]>("getSelectedRoles", roleSelect);
        //var selectedRoles = selectedRoles;
        //_userRequest.Roles = selectedRoles.ToList();
        _userRequest.Roles = selectedRoles;

        var response = await _userService.AddUser(_userRequest);
        if (response.IsSuccess)
        {
            CloseModal();
            await LoadUsersAsync();
            StateHasChanged();
            _userRequest = new UserRequest
            {
                Address = new AddressRequest(),
                PhoneNumbers = new List<PhoneNumberRequest>(),
                Roles = new()
            };
        }
        else
        {
            _alertService.ShowErrorAlert(response.Error.description);

        }

    }

    private void AddPhoneNumber()
    {
        // Add a new phone number entry
        _userRequest.PhoneNumbers?.Add(new PhoneNumberRequest());
    }

    private void RemovePhoneNumber(int index)
    {
        if (_userRequest.PhoneNumbers.Count > 1)
        {
            _userRequest.PhoneNumbers.RemoveAt(index);
        }
    }

    private void SetAsPrimary(int index)
    {
        for (int i = 0; i < _userRequest.PhoneNumbers.Count; i++)
        {
            _userRequest.PhoneNumbers[i].IsPrimary = (i == index);
        }

    }


    private async Task LoadUsersAsync()
    {
        IsLoading = true;

        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;

        UserList = await _userService.GetAllUsers(filters);

        IsLoading = false;


    }
    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            PageSize = size;
            CurrentPage = 1;
            await LoadUsersAsync();
        }
    }

    private async Task ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= UserList.TotalPages)
        {
            CurrentPage = pageNumber;
            await LoadUsersAsync();
        }
    }

}

