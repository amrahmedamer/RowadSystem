using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Reports;

namespace RowadSystem.UI.Features.Reports;

public interface IReportService
{
    Task<Result<SalesSummaryDto>> GetSalesPerDayAsync();
    Task<Result<SalesSummaryDto>> GetSalesPerMonthAsync();
    Task<Result<List<TopProductDto>>> GetTopSellingProducts();
    Task<Result<List<LowStockProductDto>>> GetLowStockProductsAsync();
    Task<Result<List<SalesSummary>>> GetDailySalesAsync(RangDateDto rangDateDto);
    Task<Result<CustomersDashboardDto>> GetCustomersAsync();
    Task<Result<OrdersDashboardDto>> GetOrdersAsync();
    Task<byte[]> ExportInventoryReportExcelAsync();


    Task<byte[]> ExportInventoryReportPdfAsync();


    Task<byte[]> GenerateSalesReportExcelAsync();
    Task<byte[]> GenerateSalesReportPdfAsync();
    Task<byte[]> GeneratePurchaseReportPdfAsync();

    Task<byte[]> GenerateSalesReturnReportPdfAsync();
    Task<byte[]> GenerateSalesReturnReportExcelAsync();
    Task<byte[]> GeneratePurchaseReturnReportPdfAsync();

    Task<byte[]> GeneratePurchaseReturnReportExcelAsync();
    Task<byte[]> GenerateProfitLossReportPdfAsync();

    Task<byte[]> GenerateProfitLossReportExcelAsync();
    Task<byte[]> SalesByEmployeeReportPdfAsync();

    Task<byte[]> SalesByEmployeeReportExcelAsync();
    Task<byte[]> SalesByCategoryReportPdfAsync();

    Task<byte[]> SalesByCategoryReportExcelAsync();

    Task<byte[]> CustomerPaymentsReportPdfAsync();
}

