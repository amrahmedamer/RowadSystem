

using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.ComponentModel;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace RowadSystem.API.Report
{
    public class SalesReportService(ApplicationDbContext context) : ISalesReporService
    {
        private readonly ApplicationDbContext _context = context;
        [Obsolete]
        public byte[] GenerateSalesReportPdf()
        {
            var salesData = _context.SalesInvoices
                .Include(s => s.SalesInvoiceDetails)
                .ThenInclude(oi => oi.Product)
                .ToList();

            var totalSales = salesData.Sum(s => s.TotalAmount);  // إجمالي المبيعات
            var totalInvoices = salesData.Count(); // عدد الفواتير المصدرة
            var totalProductsSold = salesData.Sum(s => s.SalesInvoiceDetails.Sum(oi => oi.Quantity)); // عدد المنتجات المباعة
            var products = _context.Products.ToList();

            // إنشاء التقرير باستخدام QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4); // يمكنك تحديد حجم الصفحة


                    // الهيدر يظهر فقط في الصفحة الأولى
                    page.Header()
                        .ShowOnce()
                        .Column(column =>
                        {
                            column.Item()
                                .AlignCenter()
                                .Text("Rowad System - Sales Report")
                                .FontSize(16)
                                .Bold();

                            column.Item()
                                .AlignCenter()
                                .Text($"Report Date: {DateTime.Now.ToString("yyyy-MM-dd")}")
                                .FontSize(12);

                            column.Item()
                                .AlignCenter()
                                .Text($"Total Sales: {totalSales:C}")
                                .FontSize(14);

                            column.Item()
                                .AlignCenter()
                                .Text($"Total Invoices: {totalInvoices}")
                                .FontSize(14);
                        });

                    // تجميع كل المحتوى في Column واحدة
                    page.Content()
                        .Column(column =>
                        {
                            // إضافة الجدول الخاص بالمبيعات حسب المنتج
                            column.Item()
                                .PaddingTop(30)  // فاصلة أعلى الجدول
                                .Table(table =>
                                {
                                    // تعريف الأعمدة
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn(); // أضف عمود لعدد المنتجات المباعة
                                    });

                                    // إضافة العناوين (Headers)
                                    table.Header(header =>
                                    {
                                        header.Cell().Element(CellStyle).Text("#").AlignStart();
                                        header.Cell().Element(CellStyle).Text("Product Name").AlignStart();
                                        header.Cell().Element(CellStyle).Text("Quantity Sold").AlignCenter();
                                        header.Cell().Element(CellStyle).Text("Unit Price").AlignCenter();
                                        header.Cell().Element(CellStyle).Text("Total Sales").AlignCenter();
                                    });

                                    // إضافة البيانات
                                    foreach (var product in products)
                                    {
                                        var productSales = salesData
                                              .SelectMany(s => s.SalesInvoiceDetails)
                                              .Where(oi => oi.ProductId == product.Id)
                                              .Select(oi => new { oi.Quantity, oi.Price, TotalSales = oi.Quantity * oi.Price })
                                              .ToList();

                                        // حساب إجمالي الكمية المباعة والمبيعات الإجمالية للمنتج
                                        var totalQuantitySold = productSales.Sum(x => x.Quantity);
                                        var totalSalesAmount = productSales.Sum(x => x.TotalSales);
                                        var unitPrice = productSales.FirstOrDefault()?.Price ?? 0;

                                        table.Cell().Element(CellStyle).Text(products.IndexOf(product) + 1);
                                        table.Cell().Element(CellStyle).Text(product.Name).AlignStart();
                                        table.Cell().Element(CellStyle).Text(totalQuantitySold.ToString("N2")).AlignCenter();
                                        table.Cell().Element(CellStyle).Text(unitPrice.ToString("C")).AlignCenter();
                                        table.Cell().Element(CellStyle).Text(totalSalesAmount.ToString("C")).AlignCenter();
                                    }
                                });
                        });
                });
            }).GeneratePdf(); 
          
            return document;
          
        }

       

        public byte[] GenerateSalesReportExcel()
        {
            var salesData = _context.SalesInvoices
                .Include(s => s.SalesInvoiceDetails)
                .ThenInclude(oi => oi.Product)
                .ToList();

            var totalSales = salesData.Sum(s => s.TotalAmount);  // إجمالي المبيعات
            var totalInvoices = salesData.Count(); // عدد الفواتير المصدرة
            var totalProductsSold = salesData.Sum(s => s.SalesInvoiceDetails.Sum(oi => oi.Quantity)); // عدد المنتجات المباعة
            var products = _context.Products.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Sales Report");

                // دمج الخلايا للترويسة
                worksheet.Range("A1:D1").Merge(); // دمج الخلايا من A1 إلى D1
                worksheet.Cell(1, 1).Value = "Rowad System - Sales Report"; // ترويسة التقرير

                // تنسيق الترويسة
                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.Row(1).Style.Font.FontColor = XLColor.White;
                worksheet.Cells().Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E79"); // اللون الأزرق الداكن
                worksheet.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(1).Style.Font.SetFontSize(16);
                worksheet.Row(1).Style.Font.FontName = "Arial";  // يمكنك تغيير الخط
                worksheet.Row(1).Style.Alignment.WrapText = true;

                // إضافة التاريخ في الترويسة
                worksheet.Range("A2:D2").Merge(); // دمج الخلايا لتاريخ التقرير
                worksheet.Cell(2, 1).Value = $"Report Date: {DateTime.Now.ToString("yyyy-MM-dd")}";
                worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(2, 1).Style.Font.SetFontSize(12);
                worksheet.Cell(2, 1).Style.Font.FontName = "Arial";

                //// إضافة إجمالي المبيعات
                //worksheet.Cell(3, 1).Value = "Total Sales:";
                //worksheet.Cell(3, 2).Value = totalSales;
                //worksheet.Cell(3, 2).Style.NumberFormat.Format = "#,##0.00";
                // إضافة إجمالي المبيعات
                worksheet.Range("A3:B3").Merge();  // دمج الخلايا من A3 إلى B3
                worksheet.Cell(3, 1).Value = "Total Sales:";
                worksheet.Cell(3, 1).Style.Font.Bold = true;  // جعل النص بالخط العريض
                worksheet.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;  // محاذاة النص لليسار
                worksheet.Cell(3, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");  // خلفية خفيفة

                worksheet.Range("B3:D3").Merge();
                worksheet.Cell(3, 2).Value = totalSales;
                worksheet.Cell(3, 2).Style.NumberFormat.Format = "#,##0.00";  // تنسيق الرقم
                worksheet.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;  // محاذاة الرقم لليمين
                worksheet.Cell(3, 2).Style.Font.Bold = true;  // جعل الرقم بالخط العريض
                worksheet.Cell(3, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");  // خلفية خفيفة

                worksheet.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(3, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // إضافة عدد الفواتير
                worksheet.Range("A4:B4").Merge();  // دمج الخلايا من A4 إلى B4
                worksheet.Cell(4, 1).Value = "Total Invoices:";
                worksheet.Cell(4, 1).Style.Font.Bold = true;  // جعل النص بالخط العريض
                worksheet.Cell(4, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;  // محاذاة النص لليسار
                worksheet.Cell(4, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");  // خلفية خفيفة

                worksheet.Range("B4:D4").Merge();
                worksheet.Cell(4, 2).Value = totalInvoices;
                worksheet.Cell(4, 2).Style.NumberFormat.Format = "#,##0";  // تنسيق الرقم
                worksheet.Cell(4, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;  // محاذاة الرقم لليمين
                worksheet.Cell(4, 2).Style.Font.Bold = true;  // جعل الرقم بالخط العريض
                worksheet.Cell(4, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EAD3");  // خلفية خفيفة
                worksheet.Cell(4, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(4, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                //// إضافة عدد الفواتير
                //worksheet.Cell(4, 1).Value = "Total Invoices:";
                //worksheet.Cell(4, 2).Value = totalInvoices;

                // مبيعات حسب المنتج
                worksheet.Cell(6, 1).Value = "Product Name";
                worksheet.Cell(6, 2).Value = "Quantity Sold";
                worksheet.Cell(6, 3).Value = "Unit Price";
                worksheet.Cell(6, 4).Value = "Total Sales";
                worksheet.Row(6).Style.Font.Bold = true;

                // تنسيق العناوين
                var headerRow = worksheet.Row(6);
                headerRow.Cells().Style.Font.Bold = true;
                headerRow.Cells().Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");  // لون خلفية أزرق داكن
                headerRow.Cells().Style.Font.FontColor = XLColor.White;
                headerRow.Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRow.Cells().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                headerRow.Cells().Style.Font.SetFontSize(12);
                headerRow.Cells().Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                var row = 7;
                foreach (var product in products)
                {
                    // Get the sales data for the current product
                    var productSales = salesData
                        .SelectMany(s => s.SalesInvoiceDetails)
                        .Where(oi => oi.ProductId == product.Id)
                        .Select(oi => new { oi.Quantity, oi.Price, TotalSales = oi.Quantity * oi.Price })
                        .ToList();

                    // Calculate total quantity sold and total sales for the product
                    var totalQuantitySold = productSales.Sum(x => x.Quantity);
                    var totalSalesAmount = productSales.Sum(x => x.TotalSales);

                    // Set the product name
                    worksheet.Cell(row, 1).Value = product.Name;

                    // Set the total quantity sold, total sales, and unit price
                    worksheet.Cell(row, 2).Value = totalQuantitySold;


                    // Add the unit price column (optional)
                    var unitPrice = productSales.FirstOrDefault()?.Price ?? 0;
                    worksheet.Cell(row, 3).Value = unitPrice;
                    worksheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0.00";  // Format price to show 2 decimal places

                    worksheet.Cell(row, 4).Value = totalSalesAmount;
                    worksheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";

                    // Align the values to the center of the cells
                    worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    worksheet.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    worksheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    worksheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    // Move to the next row for the next product
                    row++;
                }



                // إضافة بعض التنسيقات العامة
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().Style.Border.OutsideBorder = XLBorderStyleValues.Thin; // حدود خارجية لجميع الخلايا
                worksheet.Rows().Style.Border.InsideBorder = XLBorderStyleValues.Thin; // حدود داخلية بين الخلايا

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
}


