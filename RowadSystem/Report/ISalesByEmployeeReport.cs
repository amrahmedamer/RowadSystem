namespace RowadSystem.API.Report;

public interface ISalesByEmployeeReport
{
    byte[] GenerateSalesByEmployeeReportPdf();
    byte[] GenerateSalesByEmployeeReportExcel();
}
