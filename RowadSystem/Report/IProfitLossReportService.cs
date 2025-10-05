namespace RowadSystem.API.Report;

public interface IProfitLossReportService
{
    byte[] GenerateProfitLossReportExcel();
    byte[] GenerateProfitLossReportPdf();
}
