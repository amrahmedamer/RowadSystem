using Microsoft.AspNetCore.Components;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.ExpenseCategory;
using RowadSystem.Shard.Contract.Expenses;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.UI.Features.ExpensesCategory;


namespace RowadSystem.UI.Features.Expenses;

public partial class Expense
{
    [Inject]
    private IExpenseService _expenseService { get; set; } = default!;

    private List<ExpenseCategoryResponse> _categoryResponse = new();
    private ExpenseRequest _expenseRequest = new();

    private PaginatedListResponse<ExpenseResponse>? _listResponse=new();
    public RequestFilters filters { get; set; } = new();
    private bool IsLoading = true;
    private int CurrentPage = 1;
    private int PageSize = 10;
    private Modal modal;
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
        var response = await _expenseService.AddExpense(_expenseRequest);
        if (response.IsSuccessStatusCode)
        {
            CloseModal();
            await LoadExpense();
            StateHasChanged();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("❌ Error: " + error);
        }

    }

    protected override async Task OnInitializedAsync()
    {
        await LoadExpense();
        await LoadExpenseCategory();
    }

    private async Task LoadExpenseCategory()
    {
        IsLoading = true;
        var result = await _expenseService.GetExpenseCategoryAsync();
        _categoryResponse = result.Value;
        IsLoading = false;
    }
    private async Task LoadExpense()
    {
        IsLoading = true;

        filters.PageNumber = CurrentPage;
        filters.PageSize = PageSize;

        var result = await _expenseService.GetAllExpenseAsync(filters);
        _listResponse = result.Value;
        IsLoading = false;


    }
    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
        {
            PageSize = size;
            CurrentPage = 1;
            await LoadExpense();
        }
    }

    private async Task ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= _listResponse.TotalPages)
        {
            CurrentPage = pageNumber;
            await LoadExpense();
        }
    }
}
