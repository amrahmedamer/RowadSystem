namespace RowadSystem.API.Report;

public interface ICustomerPaymentsReportService
{
    byte[] GenerateCustomerPaymentsReportPdf();
}
