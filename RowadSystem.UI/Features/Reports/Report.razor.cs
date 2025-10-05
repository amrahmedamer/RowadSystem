using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RowadSystem.UI.Features.Reports;

public partial class Report
{
    [Inject]
    private IReportService _reportService { get; set; } = default!;
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private bool isLoading = false; // Flag to show loading spinner
    private string activeTab = "sales";  // Initial active tab

    // Method to handle the active tab switching
    private void SetActiveTab(string tab)
    {
        activeTab = tab;
    }


    // Export Methods
    private async Task ExportSalesReportPdf()
    {
        isLoading = true;
        var fileData = await _reportService.GenerateSalesReportPdfAsync();
        await DownloadFile("SalesReport.pdf", fileData);
        isLoading = false;
    }

    private async Task ExportSalesReportExcel()
    {
        isLoading = true;
        var fileData = await _reportService.GenerateSalesReportExcelAsync();
        await DownloadFile("SalesReport.xlsx", fileData);
        isLoading = false;
    }

    private async Task ExportInventoryReportPdf()
    {
        isLoading = true;
        var fileData = await _reportService.ExportInventoryReportPdfAsync();
        await DownloadFile("InventoryReport.pdf", fileData);
        isLoading = false;
    }

    private async Task ExportInventoryReportExcel()
    {
        isLoading = true;
        var fileData = await _reportService.ExportInventoryReportExcelAsync();
        await DownloadFile("InventoryReport.xlsx", fileData);
        isLoading = false;
    }

    private async Task ExportProfitLossReportPdf()
    {
        isLoading = true;
        var fileData = await _reportService.GenerateProfitLossReportPdfAsync();
        await DownloadFile("ProfitLossReport.pdf", fileData);
        isLoading = false;
    }

    private async Task ExportProfitLossReportExcel()
    {
        isLoading = true;
        var fileData = await _reportService.GenerateProfitLossReportExcelAsync();
        await DownloadFile("ProfitLossReport.xlsx", fileData);
        isLoading = false;
    }

    private async Task ExportSalesReturnReportPdf()
    {
        isLoading = true;
        var fileData = await _reportService.GenerateSalesReturnReportPdfAsync();
        await DownloadFile("SalesReturnReport.pdf", fileData);
        isLoading = false;
    }

    private async Task ExportSalesReturnReportExcel()
    {
        isLoading = true;
        var fileData = await _reportService.GenerateSalesReturnReportExcelAsync();
        await DownloadFile("SalesReturnReport.xlsx", fileData);
        isLoading = false;
    }

    private async Task ExportPurchaseReturnReportPdf()
    {
        isLoading = true;
        var fileData = await _reportService.GeneratePurchaseReturnReportPdfAsync();
        await DownloadFile("PurchaseReturnReport.pdf", fileData);
        isLoading = false;
    }

    private async Task ExportPurchaseReturnReportExcel()
    {
        isLoading = true;
        var fileData = await _reportService.GeneratePurchaseReturnReportExcelAsync();
        await DownloadFile("PurchaseReturnReport.xlsx", fileData);
        isLoading = false;
    }

    private async Task DownloadFile(string fileName, byte[] fileData)
    {
        var base64Data = Convert.ToBase64String(fileData);
        var fileUrl = $"data:application/octet-stream;base64,{base64Data}";
        await JSRuntime.InvokeVoidAsync("downloadFile", fileUrl, fileName);
    }
}
