using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Reports;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Reports;

public class ReportService(IHttpClientFactory httpClient) : IReportService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");

    public async Task<Result<List<LowStockProductDto>>> GetLowStockProductsAsync()
    {
        var response = await _httpClient.GetAsync($"/api/Reports/low-stock-product");
        var result = await response.Content.ReadFromJsonAsync<List<LowStockProductDto>>();
        return Result.Success(result);
    }

    public async Task<Result<SalesSummaryDto>> GetSalesPerDayAsync()
    {
        var response = await _httpClient.GetAsync($"/api/Reports/daily-sales");
        var result = await response.Content.ReadFromJsonAsync<SalesSummaryDto>();
        return Result.Success(result);
    }
    public async Task<Result<SalesSummaryDto>> GetSalesPerMonthAsync()
    {
        var response = await _httpClient.GetAsync($"/api/Reports/monthly-sales");
        var result = await response.Content.ReadFromJsonAsync<SalesSummaryDto>();
        return Result.Success(result);
    }
    public async Task<Result<List<TopProductDto>>> GetTopSellingProducts()
    {
        var response = await _httpClient.GetAsync($"/api/Reports/top-product");
        var result = await response.Content.ReadFromJsonAsync<List<TopProductDto>>();
        return Result.Success(result);
    }
    public async Task<Result<List<SalesSummary>>> GetDailySalesAsync(RangDateDto rangDateDto)
    {
        var response = await _httpClient.GetAsync($"/api/Reports/rang-daily-sales?startDate={rangDateDto.StartDate}&endDate={rangDateDto.EndDate}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<SalesSummary>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<List<SalesSummary>>(response);
        }
       
    }
    public async Task<Result<CustomersDashboardDto>> GetCustomersAsync()
    {
        var response = await _httpClient.GetAsync($"/api/Reports/customers");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CustomersDashboardDto>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<CustomersDashboardDto>(response);
        }
       
    }
    public async Task<Result<OrdersDashboardDto>> GetOrdersAsync()
    {
        var response = await _httpClient.GetAsync($"/api/Reports/orders");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<OrdersDashboardDto>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<OrdersDashboardDto>(response);
        }
       
    }

    public async Task<byte[]> GetFileAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<byte[]> ExportInventoryReportExcelAsync()
    {
        return await GetFileAsync("api/reports/export-inventory-report-excel");
    }

    public async Task<byte[]> ExportInventoryReportPdfAsync()
    {
        return await GetFileAsync("api/reports/export-inventory-report-pdf");
    }

    public async Task<byte[]> GenerateSalesReportExcelAsync()
    {
        return await GetFileAsync("api/reports/export-sales-report-excel");
    }

    public async Task<byte[]> GenerateSalesReportPdfAsync()
    {
        return await GetFileAsync("api/reports/export-sales-report-pdf");
    }

    public async Task<byte[]> GeneratePurchaseReportPdfAsync()
    {
        return await GetFileAsync("api/reports/export-purchase-report-pdf");
    }

    public async Task<byte[]> GenerateSalesReturnReportPdfAsync()
    {
        return await GetFileAsync("api/reports/export-sales-return-report-pdf");
    }

    public async Task<byte[]> GenerateSalesReturnReportExcelAsync()
    {
        return await GetFileAsync("api/reports/export-sales-return-report-excel");
    }

    public async Task<byte[]> GeneratePurchaseReturnReportPdfAsync()
    {
        return await GetFileAsync("api/reports/export-purchase-return-report-pdf");
    }

    public async Task<byte[]> GeneratePurchaseReturnReportExcelAsync()
    {
        return await GetFileAsync("api/reports/export-purchase-return-report-excel");
    }

    public async Task<byte[]> GenerateProfitLossReportPdfAsync()
    {
        return await GetFileAsync("api/reports/export-profit-loss-report-pdf");
    }

    public async Task<byte[]> GenerateProfitLossReportExcelAsync()
    {
        return await GetFileAsync("api/reports/export-profit-loss-report-excel");
    }

    public async Task<byte[]> SalesByEmployeeReportPdfAsync()
    {
        return await GetFileAsync("api/reports/export-sales-by-employee-report-pdf");
    }

    public async Task<byte[]> SalesByEmployeeReportExcelAsync()
    {
        return await GetFileAsync("api/reports/export-sales-by-employee-report-excel");
    }

    public async Task<byte[]> SalesByCategoryReportPdfAsync()
    {
        return await GetFileAsync("api/reports/export-sales-by-category-report-pdf");
    }

    public async Task<byte[]> SalesByCategoryReportExcelAsync()
    {
        return await GetFileAsync("api/reports/export-sales-by-category-report-excel");
    }

    public async Task<byte[]> CustomerPaymentsReportPdfAsync()
    {
        return await    GetFileAsync("api/reports/export-customer-payment-report-pdf");
    }

}
