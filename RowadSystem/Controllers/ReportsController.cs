using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Report;
using RowadSystem.API.Services;
using RowadSystem.Shard.Contract.Reports;

namespace RowadSystem.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportsController(IReportService reportService,
    ISalesReporService salesReporService,
    IPurchaseReportService purchaseReportService,
    ISalesReturnsReportService salesReturnsReportService,
    IPurchaseReturnsReportService purchaseReturnsReportService,
    IProfitLossReportService profitLossReportService,
    ISalesByEmployeeReport salesByEmployeeReport,
    ISalesByCategoryReport salesByCategoryReport,
    ICustomerPaymentsReportService customerPaymentsReportService,
    IInventoryReportService inventory) : ControllerBase
{
    private readonly IReportService _reportService = reportService;
    private readonly ISalesReporService _salesReporService = salesReporService;
    private readonly IPurchaseReportService _purchaseReportService = purchaseReportService;
    private readonly ISalesReturnsReportService _salesReturnsReportService = salesReturnsReportService;
    private readonly IPurchaseReturnsReportService _purchaseReturnsReportService = purchaseReturnsReportService;
    private readonly IProfitLossReportService _profitLossReportService = profitLossReportService;
    private readonly ISalesByEmployeeReport _salesByEmployeeReport = salesByEmployeeReport;
    private readonly ISalesByCategoryReport _salesByCategoryReport = salesByCategoryReport;
    private readonly ICustomerPaymentsReportService _customerPaymentsReportService = customerPaymentsReportService;
    private readonly IInventoryReportService _inventory = inventory;

    [HttpGet("daily-sales")]
    public async Task<IActionResult> GetSalesPerDay()
    {
        var result = await _reportService.SalesPerDay();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("monthly-sales")]
    public async Task<IActionResult> GetSalesPerMonth()
    {
        var result = await _reportService.SalesPerMonth();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("top-product")]
    public async Task<IActionResult> GetTopProducts()
    {
        var result = await _reportService.GetTopSellingProducts();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("low-stock-product")]
    public async Task<IActionResult> GetLowStockProducts()
    {
        var result = await _reportService.GetLowStockProductsAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("rang-daily-sales")]
    public async Task<IActionResult> GetDailySalesAsync([FromQuery] RangDateDto rangDateDto)
    {
        var result = await _reportService.GetDailySalesAsync(rangDateDto);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomersDashboard()
    {
        var result = await _reportService.GetCustomersDashboard();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("orders")]
    public async Task<IActionResult> GetOrdersDashboard()
    {
        var result = await _reportService.GetOrdersDashboard();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("export-inventory-report-excel")]
    public IActionResult ExportInventoryReportExcel()
    {
        var reportData = _inventory.GenerateInventoryReport();

        return File(reportData,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "InventoryReport.xlsx");
    }
    [HttpGet("export-inventory-report-pdf")]
    public IActionResult ExportInventoryReportPdf()
    {
        try
        {
            // Generate the PDF report data
            var reportData = _inventory.GenerateInventoryReportPdf();

            // Return the PDF file as a response
            return File(reportData, "application/pdf", "InventoryReport.pdf");
        }
        catch (Exception ex)
        {
            // Log the error or handle it accordingly
            // Example: _logger.LogError(ex, "Error generating inventory report");

            return StatusCode(500, new { message = "An error occurred while generating the report." });
        }
    }
    [HttpGet("export-sales-report-excel")]
    public IActionResult GenerateSalesReportExcel()
    {
        var reportData = _salesReporService.GenerateSalesReportExcel();

        return File(reportData,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "SalesReport.xlsx");
    }
    [HttpGet("export-sales-report-pdf")]
    public IActionResult GenerateSalesReportPdf()
    {
        try
        {
            // Generate the PDF report data
            var reportData = _salesReporService.GenerateSalesReportPdf();

            // Return the PDF file as a response
            return File(reportData, "application/pdf", "SalesReport.pdf");
        }
        catch (Exception ex)
        {
            // Log the error or handle it accordingly
            // Example: _logger.LogError(ex, "Error generating inventory report");

            return StatusCode(500, new { message = "An error occurred while generating the report." });
        }
    }
    [HttpGet("export-purchase-report-pdf")]
    public IActionResult GeneratePurchaseReportPdf()
    {
        try
        {
            // Generate the PDF report data
            var reportData = _purchaseReportService.GeneratePurchaseReportPdf();

            // Return the PDF file as a response
            return File(reportData, "application/pdf", "PurchaseReport.pdf");
        }
        catch (Exception ex)
        {
            // Log the error or handle it accordingly
            // Example: _logger.LogError(ex, "Error generating inventory report");

            return StatusCode(500, new { message = "An error occurred while generating the report." });
        }
    }
    [HttpGet("export-sales-return-report-pdf")]
    public IActionResult GenerateSalesReturnReportPdf()
    {
        try
        {
            // Generate the PDF report data
            var reportData = _salesReturnsReportService.GenerateSalesReturnsReportPdf();

            // Return the PDF file as a response
            return File(reportData, "application/pdf", "SalesReturnReport.pdf");
        }
        catch (Exception ex)
        {
            // Log the error or handle it accordingly
            // Example: _logger.LogError(ex, "Error generating inventory report");

            return StatusCode(500, new { message = "An error occurred while generating the report." });
        }
    }
    [HttpGet("export-sales-return-report-excel")]
    public IActionResult GenerateSalesReturnReportExcel()
    {
        var reportData = _salesReturnsReportService.GenerateSalesReturnsReportExcel();

        return File(reportData,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "SalesReturnReport.xlsx");
    }
    [HttpGet("export-purchase-return-report-pdf")]
    public IActionResult GeneratePurchaseReturnReportPdf()
    {
        try
        {
            // Generate the PDF report data
            var reportData = _purchaseReturnsReportService.GeneratePurchaseReturnsReportPdf();

            // Return the PDF file as a response
            return File(reportData, "application/pdf", "PurchaseReturnReport.pdf");
        }
        catch (Exception ex)
        {
            // Log the error or handle it accordingly
            // Example: _logger.LogError(ex, "Error generating inventory report");

            return StatusCode(500, new { message = "An error occurred while generating the report." });
        }
    }
    [HttpGet("export-purchase-return-report-excel")]
    public IActionResult GeneratePurchaseReturnReportExcel()
    {
        var reportData = _purchaseReturnsReportService.GeneratePurchaseReturnsReportExcel();

        return File(reportData,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "PurchaseReturnReport.xlsx");
    }
    [HttpGet("export-profit-loss-report-pdf")]
    public IActionResult GenerateProfitLossReportPdf()
    {
        try
        {
            // Generate the PDF report data
            var reportData = _profitLossReportService.GenerateProfitLossReportPdf();

            // Return the PDF file as a response
            return File(reportData, "application/pdf", "ProfitLossReport.pdf");
        }
        catch (Exception ex)
        {
            // Log the error or handle it accordingly
            // Example: _logger.LogError(ex, "Error generating inventory report");

            return StatusCode(500, new { message = "An error occurred while generating the report." });
        }
    }
    [HttpGet("export-profit-loss-report-excel")]
    public IActionResult GenerateProfitLossReportExcel()
    {
        var reportData = _profitLossReportService.GenerateProfitLossReportExcel();

        return File(reportData,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ProfitLossReport.xlsx");
    }
    [HttpGet("export-sales-by-employee-report-pdf")]
    public IActionResult SalesByEmployeeReportPdf()
    {
        try
        {
            // Generate the PDF report data
            var reportData = _salesByEmployeeReport.GenerateSalesByEmployeeReportPdf();

            // Return the PDF file as a response
            return File(reportData, "application/pdf", "SalesByEmployeeReport.pdf");
        }
        catch (Exception ex)
        {
            // Log the error or handle it accordingly
            // Example: _logger.LogError(ex, "Error generating inventory report");

            return StatusCode(500, new { message = "An error occurred while generating the report." });
        }
    }
    [HttpGet("export-sales-by-employee-report-excel")]
    public IActionResult SalesByEmployeeReportExcel()
    {
        var reportData = _salesByEmployeeReport.GenerateSalesByEmployeeReportExcel();

        return File(reportData,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "SalesByEmployeeReport.xlsx");
    }
    [HttpGet("export-sales-by-category-report-pdf")]
    public IActionResult SalesByCategoryReportPdf()
    {
        try
        {
            // Generate the PDF report data
            var reportData = _salesByCategoryReport.GenerateSalesByCategoryReportPdf();

            // Return the PDF file as a response
            return File(reportData, "application/pdf", "SalesByCategoryReport.pdf");
        }
        catch (Exception ex)
        {
            // Log the error or handle it accordingly
            // Example: _logger.LogError(ex, "Error generating inventory report");

            return StatusCode(500, new { message = "An error occurred while generating the report." });
        }
    }
    [HttpGet("export-sales-by-category-report-excel")]
    public IActionResult SalesByCategoryReportExcel()
    {
        var reportData = _salesByCategoryReport.GenerateSalesByCategoryReportExcel();

        return File(reportData,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "SalesByCategoryReport.xlsx");
    }
    [HttpGet("export-customer-payment-report-pdf")]
    public IActionResult CustomerPaymentsReportPdf()
    {
        try
        {
            // Generate the PDF report data
            var reportData = _customerPaymentsReportService.GenerateCustomerPaymentsReportPdf();

            // Return the PDF file as a response
            return File(reportData, "application/pdf", "CustomerPaymentsReport.pdf");
        }
        catch (Exception ex)
        {
            // Log the error or handle it accordingly
            // Example: _logger.LogError(ex, "Error generating inventory report");

            return StatusCode(500, new { message = "An error occurred while generating the report." });
        }
    }
   
}
