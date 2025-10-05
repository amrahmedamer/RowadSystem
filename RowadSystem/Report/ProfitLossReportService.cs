using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RowadSystem.API.Report;

public class ProfitLossReportService : IProfitLossReportService
{
    private readonly ApplicationDbContext _context;

    public ProfitLossReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public byte[] GenerateProfitLossReportPdf()
    {
        var salesData = _context.SalesInvoices
            .Include(s => s.SalesInvoiceDetails)
            .ThenInclude(oi => oi.Product)
            .ToList();

        var totalRevenue = salesData.Sum(s => s.TotalAmount);  // إجمالي المبيعات
        var totalCost = salesData.Sum(s => s.SalesInvoiceDetails.Sum(oi => oi.Quantity * oi.Product.PurchasePrice));  // إجمالي التكلفة
        var totalProfit = totalRevenue - totalCost;  // الأرباح الصافية

        var products = _context.Products.ToList();

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
                            .Text("Profit and Loss Report")
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
                        // عرض إجمالي الإيرادات والتكاليف
                        column.Item()
                            .PaddingTop(30)
                            .Text($"Total Revenue: {totalRevenue:C}")
                            .FontSize(14)
                            .Bold();
                        column.Item()
                            .Text($"Total Cost: {totalCost:C}")
                            .FontSize(14)
                            .Bold();
                        column.Item()
                            .Text($"Net Profit: {totalProfit:C}")
                            .FontSize(14)
                            .Bold();

                        // جدول تحليل الأرباح حسب المنتج
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
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Product Name").AlignStart();
                                    header.Cell().Element(CellStyle).Text("Revenue").AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Cost").AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Profit").AlignCenter();
                                });

                                foreach (var product in products)
                                {
                                    var productSales = salesData
                                        .SelectMany(s => s.SalesInvoiceDetails)
                                        .Where(oi => oi.ProductId == product.Id)
                                        .Select(oi => new
                                        {
                                            Revenue = oi.Quantity * oi.Price,
                                            Cost = oi.Quantity * product.PurchasePrice,
                                            Profit = (oi.Quantity * oi.Price) - (oi.Quantity * product.PurchasePrice)
                                        })
                                        .ToList();

                                    var totalRevenueForProduct = productSales.Sum(x => x.Revenue);
                                    var totalCostForProduct = productSales.Sum(x => x.Cost);
                                    var totalProfitForProduct = totalRevenueForProduct - totalCostForProduct;

                                    table.Cell().Element(CellStyle).Text(product.Name);
                                    table.Cell().Element(CellStyle).Text(totalRevenueForProduct.ToString("C")).AlignCenter();
                                    table.Cell().Element(CellStyle).Text(totalCostForProduct.ToString("C")).AlignCenter();
                                    table.Cell().Element(CellStyle).Text(totalProfitForProduct.ToString("C")).AlignCenter();
                                }
                            });
                    });
            });
        }).GeneratePdf();

        return document;
    }


    public byte[] GenerateProfitLossReportExcel()
    {
        var salesData = _context.SalesInvoices
            .Include(s => s.SalesInvoiceDetails)
            .ThenInclude(oi => oi.Product)
            .ToList();

        var totalRevenue = salesData.Sum(s => s.TotalAmount);
        var totalCost = salesData.Sum(s => s.SalesInvoiceDetails.Sum(oi => oi.Quantity * oi.Product.PurchasePrice));
        var totalProfit = totalRevenue - totalCost;

        var products = _context.Products.ToList();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.AddWorksheet("Profit and Loss Report");

            // Merge cells for header
            worksheet.Range("A1:D1").Merge();
            worksheet.Cell(1, 1).Value = "Rowad System - Profit and Loss Report"; // Report header

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
            worksheet.Range("A2:D2").Merge();
            worksheet.Cell(2, 1).Value = $"Report Date: {DateTime.Now:yyyy-MM-dd}";
            worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(2, 1).Style.Font.SetFontSize(12);
            worksheet.Cell(2, 1).Style.Font.FontName = "Arial";

            // Add Total Revenue
            worksheet.Range("A3:B3").Merge();
            worksheet.Cell(3, 1).Value = "Total Revenue:";
            worksheet.Cell(3, 1).Style.Font.Bold = true;
            worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell(3, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3"); // Light background

            worksheet.Range("B3:D3").Merge();
            worksheet.Cell(3, 2).Value = totalRevenue;
            worksheet.Cell(3, 2).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell(3, 2).Style.Font.Bold = true;
            worksheet.Cell(3, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");

            // Add Total Cost
            worksheet.Range("A4:B4").Merge();
            worksheet.Cell(4, 1).Value = "Total Cost:";
            worksheet.Cell(4, 1).Style.Font.Bold = true;
            worksheet.Cell(4, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell(4, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");

            worksheet.Range("B4:D4").Merge();
            worksheet.Cell(4, 2).Value = totalCost;
            worksheet.Cell(4, 2).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell(4, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell(4, 2).Style.Font.Bold = true;
            worksheet.Cell(4, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");

            // Add Net Profit
            worksheet.Range("A5:B5").Merge();
            worksheet.Cell(5, 1).Value = "Net Profit:";
            worksheet.Cell(5, 1).Style.Font.Bold = true;
            worksheet.Cell(5, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell(5, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");

            worksheet.Range("B5:D5").Merge();
            worksheet.Cell(5, 2).Value = totalProfit;
            worksheet.Cell(5, 2).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell(5, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            worksheet.Cell(5, 2).Style.Font.Bold = true;
            worksheet.Cell(5, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");

            // Product Breakdown Section
            worksheet.Cell(7, 1).Value = "Product Name";
            worksheet.Cell(7, 2).Value = "Revenue";
            worksheet.Cell(7, 3).Value = "Cost";
            worksheet.Cell(7, 4).Value = "Profit";
            worksheet.Row(7).Style.Font.Bold = true;

            var headerRow = worksheet.Row(7);
            headerRow.Cells().Style.Font.Bold = true;
            headerRow.Cells().Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");  // Dark blue background
            headerRow.Cells().Style.Font.FontColor = XLColor.White;
            headerRow.Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRow.Cells().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRow.Cells().Style.Font.SetFontSize(12);
            headerRow.Cells().Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            var row = 8;
            foreach (var product in products)
            {
                var productSales = salesData
                    .SelectMany(s => s.SalesInvoiceDetails)
                    .Where(oi => oi.ProductId == product.Id)
                    .Select(oi => new
                    {
                        Revenue = oi.Quantity * oi.Price,
                        Cost = oi.Quantity * product.PurchasePrice,
                        Profit = (oi.Quantity * oi.Price) - (oi.Quantity * product.PurchasePrice)
                    })
                    .ToList();

                var totalRevenueForProduct = productSales.Sum(x => x.Revenue);
                var totalCostForProduct = productSales.Sum(x => x.Cost);
                var totalProfitForProduct = totalRevenueForProduct - totalCostForProduct;

                worksheet.Cell(row, 1).Value = product.Name;
                worksheet.Cell(row, 2).Value = totalRevenueForProduct;
                worksheet.Cell(row, 3).Value = totalCostForProduct;
                worksheet.Cell(row, 4).Value = totalProfitForProduct;

                // Align the values to the center of the cells
                worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(row, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(row, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(row, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(row, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                row++;
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


    // تنسيق الخلايا في PDF
    static IContainer CellStyle(IContainer container)
    {
        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
    }
}
