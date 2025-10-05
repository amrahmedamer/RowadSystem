

namespace RowadSystem.API.Report
{
    using ClosedXML.Excel;
    using Microsoft.EntityFrameworkCore;
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;
    using RowadSystem.Entity;
    using System;
    using System.IO;
    using System.Linq;

    public class InventoryReportService : IInventoryReportService
    {
        private readonly ApplicationDbContext _context;

        public InventoryReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public byte[] GenerateInventoryReport()
        {
            // الحصول على المنتجات مع مخزوناتها
            var products = _context.Products
                .Include(p => p.Inventory)  // تضمين معلومات المخزون
                .ToList();

            // إنشاء ملف Excel
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Inventory Report");

                // دمج الخلايا للترويسة
                worksheet.Range("A1:C1").Merge(); // دمج الخلايا من A1 إلى D1
                worksheet.Cell(1, 1).Value = "Rowad System - Inventory Report"; // ترويسة التقرير

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
                worksheet.Range("A2:C2").Merge(); // دمج الخلايا لتاريخ التقرير
                worksheet.Cell(2, 1).Value = $"Report Date: {DateTime.Now.ToString("yyyy-MM-dd")}";
                worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(2, 1).Style.Font.SetFontSize(12);
                worksheet.Cell(2, 1).Style.Font.FontName = "Arial";

                // إضافة العناوين (Headers) للمحتوى
                worksheet.Cell(3, 1).Value = "Product Name";
                worksheet.Cell(3, 2).Value = "Available Quantity";
                worksheet.Cell(3, 3).Value = "Sold Quantity";

                // تنسيق العناوين
                var headerRow = worksheet.Row(3);
                headerRow.Cells().Style.Font.Bold = true;
                headerRow.Cells().Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");  // لون خلفية أزرق داكن
                headerRow.Cells().Style.Font.FontColor = XLColor.White;
                headerRow.Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRow.Cells().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                headerRow.Cells().Style.Font.SetFontSize(12);
                headerRow.Cells().Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                // إضافة البيانات
                for (int i = 0; i < products.Count; i++)
                {
                    var inventory = products[i].Inventory;

                    worksheet.Cell(i + 4, 1).Value = products[i].Name;
                    worksheet.Cell(i + 4, 2).Value = inventory?.Quantity ?? 0;
                    worksheet.Cell(i + 4, 3).Value = GetSoldQuantity(products[i].Id);

                    // محاذاة القيم في وسط الخلية
                    worksheet.Cell(i + 4, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(i + 4, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    worksheet.Cell(i + 4, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(i + 4, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    worksheet.Cell(i + 4, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(i + 4, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                    // تنسيق الأرقام في الأعمدة
                    worksheet.Cell(i + 4, 2).Style.NumberFormat.Format = "#,##0";  // الكمية المتاحة
                    worksheet.Cell(i + 4, 3).Style.NumberFormat.Format = "#,##0";  // الكمية المباعة
                }

                // إضافة بعض التنسيقات العامة
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().Style.Border.OutsideBorder = XLBorderStyleValues.Thin; // حدود خارجية لجميع الخلايا
                worksheet.Rows().Style.Border.InsideBorder = XLBorderStyleValues.Thin; // حدود داخلية بين الخلايا

                // حفظ الملف في الذاكرة وإرجاعه كـ byte array
                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

     



        [Obsolete]
        public byte[] GenerateInventoryReportPdf()
        {
            // الحصول على المنتجات مع مخزوناتها
            var products = _context.Products
                .Include(p => p.Inventory)  // تضمين معلومات المخزون
                .ToList();

            // إنشاء التقرير باستخدام QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(595, 842); // الحجم A4 بالـ points (595 x 842)

                    // الترويسة الموحدة
                    page.Header()
                        .AlignCenter()
                        .Text("Rowad System - Inventory Report")
                        .FontSize(16)
                        .Bold();

                    // إضافة التاريخ داخل نفس الترويسة أو داخل المحتوى
                    page.Content()
                        .PaddingTop(20)
                        .Table(table =>
                        {
                            // تعريف الأعمدة
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                          
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#").AlignStart();
                                header.Cell().Element(CellStyle).Text("Product Name").AlignStart();
                                header.Cell().Element(CellStyle).AlignRight().Text("Available Quantity").AlignCenter();
                                header.Cell().Element(CellStyle).AlignRight().Text("Sold Quantit").AlignCenter();

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            foreach (var product in products)
                            {
                                var inventory = product.Inventory;
                                table.Cell().Element(CellStyle).Text(products.IndexOf(product) + 1);
                                table.Cell().Element(CellStyle).Text(product.Name).AlignStart();
                                table.Cell().Element(CellStyle).AlignRight().Text(inventory?.Quantity.ToString() ?? "0").AlignStart();
                                table.Cell().Element(CellStyle).AlignRight().Text(GetSoldQuantity(product.Id).ToString()).AlignStart();

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                }
                            }
                        });
                });
            });

            // حفظ التقرير في الذاكرة وإرجاعه كـ byte array
            using (var ms = new MemoryStream())
            {
                document.GeneratePdf(ms);
                return ms.ToArray();
            }
        }



        // دالة لحساب الكمية المباعة
        private int GetSoldQuantity(int productId)
        {
            var soldQuantity = _context.OrderItems
                .Where(oi => oi.ProductId == productId)
                .Sum(oi => oi.Quantity);
            return soldQuantity;
        }
    }
}

