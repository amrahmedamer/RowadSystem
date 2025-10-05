using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace RowadSystem.API.Report;

public class SalesByCategoryReport(ApplicationDbContext context): ISalesByCategoryReport
{
    private readonly ApplicationDbContext _context = context;

    public byte[] GenerateSalesByCategoryReportPdf()
    {
        var salesData = _context.SalesInvoices
            .Include(s => s.SalesInvoiceDetails)
            .ThenInclude(oi => oi.Product)
            .ToList();

        var categories = _context.Categories.ToList();

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
                            .Text("Sales by Category Report")
                            .FontSize(16)
                            .Bold();
                        column.Item()
                            .AlignCenter()
                            .Text($"Date: {DateTime.Now.ToString("yyyy-MM-dd")}")
                            .FontSize(12);
                    });

                page.Content()
                    .Column(column =>
                    {
                        column.Item()
                            .PaddingTop(30)
                            .Text("Sales by Category")
                            .FontSize(14)
                            .Bold();

                        // Table of sales by category
                        column.Item()
                            .PaddingTop(30)
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Category Name").AlignStart();
                                    header.Cell().Element(CellStyle).Text("Total Sales").AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Number of Products").AlignCenter();
                                });

                                foreach (var category in categories)
                                {
                                    var categorySales = salesData
                                        .SelectMany(s => s.SalesInvoiceDetails)
                                        .Where(oi => oi.Product.CategoryId == category.Id)
                                        .Sum(oi => oi.Quantity * oi.Price);

                                    var productCount = salesData
                                        .SelectMany(s => s.SalesInvoiceDetails)
                                        .Where(oi => oi.Product.CategoryId == category.Id)
                                        .Select(oi => oi.ProductId)
                                        .Distinct()
                                        .Count();

                                    table.Cell().Element(CellStyle).Text(category.Name);
                                    table.Cell().Element(CellStyle).Text(categorySales.ToString("C")).AlignCenter();
                                    table.Cell().Element(CellStyle).Text(productCount.ToString()).AlignCenter();
                                }
                            });
                    });
            });
        }).GeneratePdf();

        return document;
    }
    // تنسيق الخلايا في PDF
    static IContainer CellStyle(IContainer container)
    {
        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
    }

    public byte[] GenerateSalesByCategoryReportExcel()
    {
        var salesData = _context.SalesInvoices
            .Include(s => s.SalesInvoiceDetails)
            .ThenInclude(oi => oi.Product)
            .ToList();

        var categories = _context.Categories.ToList();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.AddWorksheet("Sales by Category Report");

            worksheet.Range("A1:C1").Merge();
            worksheet.Cell(1, 1).Value = "Sales by Category Report";
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.Font.FontColor = XLColor.White;
            worksheet.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cells().Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E79");

            worksheet.Cell(2, 1).Value = $"Report Date: {DateTime.Now.ToString("yyyy-MM-dd")}";
            worksheet.Range("A2:C2").Merge();
            worksheet.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(4, 1).Value = "Category Name";
            worksheet.Cell(4, 2).Value = "Total Sales";
            worksheet.Cell(4, 3).Value = "Number of Products";
            worksheet.Row(4).Style.Font.Bold = true;

            var row = 5;
            foreach (var category in categories)
            {
                var categorySales = salesData
                    .SelectMany(s => s.SalesInvoiceDetails)
                    .Where(oi => oi.Product.CategoryId == category.Id)
                    .Sum(oi => oi.Quantity * oi.Price);

                var productCount = salesData
                    .SelectMany(s => s.SalesInvoiceDetails)
                    .Where(oi => oi.Product.CategoryId == category.Id)
                    .Select(oi => oi.ProductId)
                    .Distinct()
                    .Count();

                worksheet.Cell(row, 1).Value = category.Name;
                worksheet.Cell(row, 2).Value = categorySales;
                worksheet.Cell(row, 3).Value = productCount;

                worksheet.Cell(row, 2).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Row(row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.Rows().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Rows().Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            using (var ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                return ms.ToArray();
            }
        }
    }


}
