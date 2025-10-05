namespace RowadSystem.API.Report;

public interface IPurchaseReturnsReportService
{
    byte[] GeneratePurchaseReturnsReportExcel();
    byte[] GeneratePurchaseReturnsReportPdf();
}
