using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RowadSystem.API.Report;

public class PurchaseReturnsReportService : IPurchaseReturnsReportService
{
    private readonly ApplicationDbContext _context;

    public PurchaseReturnsReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public byte[] GeneratePurchaseReturnsReportPdf()
    {
        var purchaseReturnData = _context.PurchaseReturns
            .Include(pi => pi.Supplier)
            .Include(pr => pr.PurchaseReturnDetails)
            .ThenInclude(p => p.Product)
            .ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header()
                    .ShowOnce()
                    .Column(column =>
                    {
                        column.Item()
                            .AlignCenter()
                            .Text("Purchase Returns Report")
                            .FontSize(16)
                            .Bold();
                        column.Item()
                            .AlignCenter()
                            .Text($"Date: {DateTime.Now:yyyy-MM-dd}")
                            .FontSize(12);
                    });

                page.Content()
                    .Column(column =>
                    {
                        column.Item()
                            .PaddingTop(30)
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Return ID").AlignStart();
                                    header.Cell().Element(CellStyle).Text("Supplier Name").AlignStart();
                                    header.Cell().Element(CellStyle).Text("Product Name").AlignStart();
                                    header.Cell().Element(CellStyle).Text("Quantity Returned").AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Return Reason").AlignStart();
                                });

                                foreach (var returnItem in purchaseReturnData)
                                {
                                    foreach (var detail in returnItem.PurchaseReturnDetails)
                                    {
                                        table.Cell().Element(CellStyle).Text(returnItem.Id.ToString());
                                        table.Cell().Element(CellStyle).Text(returnItem.Supplier?.Name??"N/A");
                                        table.Cell().Element(CellStyle).Text(detail.Product?.Name ?? "No Name");
                                        table.Cell().Element(CellStyle).Text(detail.Quantity.ToString()).AlignCenter();
                                        table.Cell().Element(CellStyle).Text(returnItem.Notes ?? "No reason provided");
                                    }
                                }
                            });
                    });
            });
        }).GeneratePdf();

        return document;
    }

    public byte[] GeneratePurchaseReturnsReportExcel()
    {
        var purchaseReturnData = _context.PurchaseReturns
            .Include(pi => pi.Supplier)
            .Include(pr => pr.PurchaseReturnDetails)
            .ThenInclude(p => p.Product)
            .ToList();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.AddWorksheet("Purchase Returns Report");

            // Merge cells for header
            worksheet.Range("A1:E1").Merge();
            worksheet.Cell(1, 1).Value = "Rowad System - Purchase Returns Report"; // Report header

            // Header formatting
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.Font.FontColor = XLColor.White;
            worksheet.Cells().Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E79"); // Dark blue color
            worksheet.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Row(1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(1).Style.Font.SetFontSize(16);
            worksheet.Row(1).Style.Font.FontName = "Arial";  // Font style
            worksheet.Row(1).Style.Alignment.WrapText = true;

            // Add report date
            worksheet.Range("A2:E2").Merge();
            worksheet.Cell(2, 1).Value = $"Report Date: {DateTime.Now:yyyy-MM-dd}";
            worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(2, 1).Style.Font.SetFontSize(12);
            worksheet.Cell(2, 1).Style.Font.FontName = "Arial";

            // Header for purchase return details
            worksheet.Cell(3, 1).Value = "Return ID";
            worksheet.Cell(3, 2).Value = "Supplier Name";
            worksheet.Cell(3, 3).Value = "Product Name";
            worksheet.Cell(3, 4).Value = "Quantity Returned";
            worksheet.Cell(3, 5).Value = "Return Reason";
            worksheet.Row(3).Style.Font.Bold = true;

            // Header row styling
            var headerRow = worksheet.Row(3);
            headerRow.Cells().Style.Font.Bold = true;
            headerRow.Cells().Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");  // Dark blue background
            headerRow.Cells().Style.Font.FontColor = XLColor.White;
            headerRow.Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRow.Cells().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRow.Cells().Style.Font.SetFontSize(12);
            headerRow.Cells().Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            var row = 4;
            foreach (var returnItem in purchaseReturnData)
            {
                foreach (var detail in returnItem.PurchaseReturnDetails)
                {
                    // Fill in the return details
                    worksheet.Cell(row, 1).Value = returnItem.Id.ToString();
                    worksheet.Cell(row, 2).Value = returnItem.Supplier?.Name ?? "N/A"; // Default if null
                    worksheet.Cell(row, 3).Value = detail.Product?.Name ?? "No Name"; // Default if null
                    worksheet.Cell(row, 4).Value = detail.Quantity.ToString(); // Default to 0 if null
                    worksheet.Cell(row, 5).Value = returnItem.Notes ?? "No reason provided"; // Default if null

                    // Align the values to the center of the cells
                    worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    worksheet.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    worksheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    worksheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    worksheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    row++;
                }
            }

            // General styling for all columns
            worksheet.Columns().AdjustToContents(); // Auto adjust column width
            worksheet.Rows().Style.Border.OutsideBorder = XLBorderStyleValues.Thin; // Outside border
            worksheet.Rows().Style.Border.InsideBorder = XLBorderStyleValues.Thin; // Inside borders

            using (var ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                return ms.ToArray();
            }
        }
    }

    // PDF Cell Style formatting
    static IContainer CellStyle(IContainer container)
    {
        return container.BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).PaddingVertical(5);
    }
}
