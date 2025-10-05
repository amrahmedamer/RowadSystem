namespace RowadSystem.API.Report;

public interface IInventoryReportService
{
    byte[] GenerateInventoryReport();
    byte[] GenerateInventoryReportPdf();
}
