namespace RowadSystem.API.Services;

public interface IExcelService
{
    byte[] ExportProductsToExcel(List<Product> products);
}
