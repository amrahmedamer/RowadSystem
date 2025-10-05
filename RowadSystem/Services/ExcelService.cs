using ClosedXML.Excel;
using RowadSystem.API.Services;

public class ExcelService: IExcelService
{
    public byte[] ExportProductsToExcel(List<Product> products)
    {
        using (var workbook = new XLWorkbook())
        {
            // Create a worksheet
            var worksheet = workbook.AddWorksheet("Products");

            // Add headers
            worksheet.Cell(1, 1).Value = "Product ID";
            worksheet.Cell(1, 2).Value = "Product Name";
            worksheet.Cell(1, 3).Value = "Price";
            worksheet.Cell(1, 4).Value = "Quantity";

            // Add product data
            for (int i = 0; i < products.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = products[i].Id;
                worksheet.Cell(i + 2, 2).Value = products[i].Name;
            }

            // Save the workbook to a MemoryStream
            using (var ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                return ms.ToArray();  // Return byte array of the Excel file
            }
        }
    }
}
