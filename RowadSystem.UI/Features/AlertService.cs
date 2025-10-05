using Microsoft.JSInterop;

namespace RowadSystem.UI.Features;

public class AlertService
{
    private readonly IJSRuntime _jsruntime;

    public AlertService(IJSRuntime jsruntime)
    {
        _jsruntime = jsruntime;
    }

    public void ShowErrorAlert(string description)
    {
        _jsruntime.InvokeVoidAsync("showSweetAlert", "Error", description, "error", "Try Again");
    }

    public void ShowSuccessAlert(string description)
    {
        _jsruntime.InvokeVoidAsync("showSweetAlert", "Success", description, "success", "OK");
    }
}
