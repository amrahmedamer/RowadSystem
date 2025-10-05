namespace RowadSystem.API.Report;

public interface ISalesByCategoryReport
{
    byte[] GenerateSalesByCategoryReportPdf();
    byte[] GenerateSalesByCategoryReportExcel();
}
