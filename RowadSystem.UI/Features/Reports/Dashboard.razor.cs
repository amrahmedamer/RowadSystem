using Blazorise.Charts;
using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Reports;
using static System.Net.WebRequestMethods;

namespace RowadSystem.UI.Features.Reports;

public partial class Dashboard
{
    [Inject]
    private IReportService _reportService { get; set; } = default!;
    private SalesSummaryDto todaySales = new();
    private SalesSummaryDto? monthlySales;
    private List<TopProductDto>? _topProductDtos;
    private List<LowStockProductDto>? _lowStockProducts;
    private string errorMessage = string.Empty;
    private RangDateDto _dateDto = new RangDateDto();
    private List<SalesSummary> salesData = new();
    private LineChart<double>? lineChart;
    private CustomersDashboardDto customersDashboard;
    private OrdersDashboardDto  _dashboardDto;
    protected override async Task OnInitializedAsync()
    {
        await LoadPerMonth();
        await LoadPerDay();
        await LoadTopProduct();
        await LoadLowStockProduct();
        await LoadData();
        await LoadCustomers();
        await LoadOrders();

    }

    public async Task LoadPerMonth()
    {
        try
        {
            var todayResult = await _reportService.GetSalesPerDayAsync();
            if (todayResult.IsSuccess)
                todaySales = todayResult.Value;

            var monthlyResult = await _reportService.GetSalesPerMonthAsync();
            if (monthlyResult.IsSuccess)
                monthlySales = monthlyResult.Value;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
    }
    public async Task LoadPerDay()
    {
        try
        {
            var result = await _reportService.GetSalesPerDayAsync();

            if (result is not null && result.IsSuccess)
                todaySales = result.Value;
            else
                errorMessage = result?.Error.description ?? "No data found";
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
    }
 
    public async Task LoadTopProduct()
    {
        try
        {
            var result = await _reportService.GetTopSellingProducts();

            if (result is not null && result.IsSuccess)
                _topProductDtos = result.Value;
            else
                errorMessage = result?.Error.description ?? "No data found";
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
    }
    public async Task LoadLowStockProduct()
    {
        try
        {
            var result = await _reportService.GetLowStockProductsAsync();

            if (result is not null && result.IsSuccess)
                _lowStockProducts = result.Value;
            else
                errorMessage = result?.Error.description ?? "No data found";
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
    }


    private async Task OnDateChanged()
    {
        await LoadData();
    }

    private async Task LoadData()
    {

        var result = await _reportService.GetDailySalesAsync(_dateDto);
        salesData = result.Value ?? new List<SalesSummary>();
        StateHasChanged();
    }
    private async Task LoadCustomers()
    {

        var result = await _reportService.GetCustomersAsync();
        customersDashboard = result.Value ?? new CustomersDashboardDto();
        StateHasChanged();
    }
    private async Task LoadOrders()
    {

        var result = await _reportService.GetOrdersAsync();
        _dashboardDto = result.Value ?? new OrdersDashboardDto();
        StateHasChanged();
    }
  
}


