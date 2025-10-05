namespace RowadSystem.API.Report;

public interface ISalesReporService
{
    byte[] GenerateSalesReportPdf();
    byte[] GenerateSalesReportExcel();
}
