namespace RowadSystem.API.Report;

public interface ISalesReturnsReportService
{
    byte[] GenerateSalesReturnsReportExcel();
    byte[] GenerateSalesReturnsReportPdf();
}
