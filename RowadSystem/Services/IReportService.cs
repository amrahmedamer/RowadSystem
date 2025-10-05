using RowadSystem.Shard.Contract.Reports;

namespace RowadSystem.API.Services;

public interface IReportService
{
    Task<Result<SalesSummaryDto>> SalesPerDay();
    Task<Result<SalesSummaryDto>> SalesPerMonth();
    Task<Result<List<TopProductDto>>> GetTopSellingProducts();
    Task<Result<List<LowStockProductDto>>> GetLowStockProductsAsync(int threshold = 10);
    Task<Result<List<SalesSummary>>> GetDailySalesAsync(RangDateDto rangDateDto);
    Task<Result<CustomersDashboardDto>> GetCustomersDashboard();
    Task<Result<OrdersDashboardDto>> GetOrdersDashboard();
}
