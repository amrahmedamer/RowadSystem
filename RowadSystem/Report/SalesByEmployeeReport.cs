using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

namespace RowadSystem.API.Report
{
    public class SalesByEmployeeReport : ISalesByEmployeeReport
    {
        private readonly ApplicationDbContext _context;

        public SalesByEmployeeReport(ApplicationDbContext context)
        {
            _context = context;
        }

        // PDF Generation Method
        public byte[] GenerateSalesByEmployeeReportPdf()
        {
            var salesData = _context.SalesInvoices
                .Include(s => s.SalesInvoiceDetails)
                .ThenInclude(oi => oi.Product)
                .Include(s => s.User) // assuming there's an Employee reference in the SalesInvoice
                .ToList();

            var employees = _context.Users.ToList();

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
                                .Text("Sales by Employee Report")
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
                                .Text("Sales by Employee")
                                .FontSize(14)
                                .Bold();

                            // Table of sales by employee
                            column.Item()
                                .PaddingTop(30)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(CellStyle).Text("Employee Name").AlignStart();
                                        header.Cell().Element(CellStyle).Text("Total Sales").AlignCenter();
                                    });

                                    foreach (var employee in employees)
                                    {
                                        var employeeSales = salesData
                                            .Where(s => s.User != null && s.User.Id == employee.Id)
                                            .Sum(s => s.TotalAmount);

                                        //var employeeCommission = salesData
                                        //    .Where(s => s.User.Id == employee.Id)
                                        //    .Sum(s => s.Commission ?? 0);  // Assuming Commission field exists

                                        table.Cell().Element(CellStyle).Text(employee.FirstName);
                                        table.Cell().Element(CellStyle).Text(employeeSales.ToString("C")).AlignCenter();
                                        //table.Cell().Element(CellStyle).Text(employeeCommission.ToString("C")).AlignCenter();
                                    }
                                });
                        });
                });
            }).GeneratePdf();

            return document;
        }

        // Excel Generation Method
        public byte[] GenerateSalesByEmployeeReportExcel()
        {
            var salesData = _context.SalesInvoices
                .Include(s => s.SalesInvoiceDetails)
                .ThenInclude(oi => oi.Product)
                .Include(s => s.User) // assuming there's an Employee reference in the SalesInvoice
                .ToList();

            var employees = _context.Users.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Sales by Employee Report");

                worksheet.Range("A1:B1").Merge();
                worksheet.Cell(1, 1).Value = "Sales by Employee Report";
                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.Row(1).Style.Font.FontColor = XLColor.White;
                worksheet.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cells().Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E79");

                worksheet.Cell(2, 1).Value = $"Report Date: {DateTime.Now:yyyy-MM-dd}";
                worksheet.Range("A2:B2").Merge();
                worksheet.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell(4, 1).Value = "Employee Name";
                worksheet.Cell(4, 2).Value = "Total Sales";
                //worksheet.Cell(4, 3).Value = "Total Commission"; // Assuming Commission field exists
                worksheet.Row(4).Style.Font.Bold = true;

                var row = 5;
                foreach (var employee in employees)
                {
                    var employeeSales = salesData
                        .Where(s => s.User != null && s.User.Id == employee.Id)
                        .Sum(s => s.TotalAmount);

                    //var employeeCommission = salesData
                    //    .Where(s => s.User != null && s.User.Id == employee.Id)
                    //    .Sum(s => s.Commission ?? 0); // Ensure Commission is not null

                    var employeeName = employee.FirstName ?? "N/A";

                    worksheet.Cell(row, 1).Value = employeeName;
                    worksheet.Cell(row, 2).Value = employeeSales;
                    //worksheet.Cell(row, 3).Value = employeeCommission;

                    worksheet.Cell(row, 2).Style.NumberFormat.Format = "#,##0.00";
                    //worksheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0.00";

                    row++;
                }

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

        // Helper method for cell styling in the PDF
        static IContainer CellStyle(IContainer container)
        {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
        }
    }
}
