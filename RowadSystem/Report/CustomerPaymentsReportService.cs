//using QuestPDF.Fluent;
//using QuestPDF.Helpers;
//using QuestPDF.Infrastructure;
//using RowadSystem.API.Report;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//public class CustomerPaymentsReportService : ICustomerPaymentsReportService
//{
//    private readonly ApplicationDbContext _context;

//    public CustomerPaymentsReportService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    [Obsolete]
//    public byte[] GenerateCustomerPaymentsReportPdf()
//    {
//        // الحصول على البيانات
//        var customerPaymentsData = _context.SalesInvoices
//            .Include(s => s.Customer)
//            .Where(s => s.PaymentStatus == PaymentStatus.Paid) // أو يمكنك تصفية حسب حالة الدفع
//            .ToList();

//        var document = Document.Create(container =>
//        {
//            container.Page(page =>
//            {
//                page.Margin(30);
//                page.Size(PageSizes.A4);

//                page.Header()
//                    .ShowOnce()
//                    .Column(column =>
//                    {
//                        column.Item()
//                            .AlignCenter()
//                            .Text("Customer Payments Report")
//                            .FontSize(16)
//                            .Bold();
//                        column.Item()
//                            .AlignCenter()
//                            .Text($"Date: {DateTime.Now.ToString("yyyy-MM-dd")}")
//                            .FontSize(12);
//                    });

//                page.Content()
//                    .Column(column =>
//                    {
//                        // جدول المدفوعات
//                        column.Item()
//                            .PaddingTop(30)
//                            .Table(table =>
//                            {
//                                table.ColumnsDefinition(columns =>
//                                {
//                                    columns.RelativeColumn();
//                                    columns.RelativeColumn();
//                                    columns.RelativeColumn();
//                                    columns.RelativeColumn();
//                                    columns.RelativeColumn();
//                                });

//                                table.Header(header =>
//                                {
//                                    header.Cell().Element(CellStyle).Text("Customer Name").AlignStart();
//                                    header.Cell().Element(CellStyle).Text("Invoice Number").AlignCenter();
//                                    header.Cell().Element(CellStyle).Text("Amount Paid").AlignCenter();
//                                    header.Cell().Element(CellStyle).Text("Remaining Amount").AlignCenter();
//                                    header.Cell().Element(CellStyle).Text("Payment Date").AlignCenter();
//                                });

//                                foreach (var invoice in customerPaymentsData)
//                                {
//                                    var totalPaid = invoice.Payments.ToList();
//                                    var paidAmount = totalPaid.Sum(p => p.Amount);
//                                    var remainingAmount = invoice.TotalAmount - paidAmount;

//                                    table.Cell().Element(CellStyle).Text(invoice.Customer.Name);
//                                    table.Cell().Element(CellStyle).Text(invoice.InvoiceNumber).AlignCenter();
//                                    table.Cell().Element(CellStyle).Text(paidAmount);
//                                    table.Cell().Element(CellStyle).Text(remainingAmount.ToString("C")).AlignCenter();
//                                    table.Cell().Element(CellStyle).Text(invoice.InvoiceDate.ToString("yyyy-MM-dd") ?? "N/A").AlignCenter();
//                                }
//                            });
//                    });
//            });
//        }).GeneratePdf();

//        return document;
//    }

//    // تنسيق الخلايا في PDF
//    static IContainer CellStyle(IContainer container)
//    {
//        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
//    }
//}


using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RowadSystem.API.Report;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class CustomerPaymentsReportService : ICustomerPaymentsReportService
{
    private readonly ApplicationDbContext _context;

    public CustomerPaymentsReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    [Obsolete]
    public byte[] GenerateCustomerPaymentsReportPdf()
    {
        // الحصول على البيانات من الفواتير المدفوعة
        var customerPaymentsData = _context.SalesInvoices
            .Include(s => s.Customer)
            .Where(s => s.PaymentStatus == PaymentStatus.Paid) // أو يمكنك تصفية حسب حالة الدفع
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
                            .Text("Customer Payments Report")
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
                        // جدول المدفوعات
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
                                    header.Cell().Element(CellStyle).Text("Customer Name").AlignStart();
                                    header.Cell().Element(CellStyle).Text("Invoice Number").AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Amount Paid").AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Remaining Amount").AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Payment Date").AlignCenter();
                                });

                                // Iterate over each invoice
                                foreach (var invoice in customerPaymentsData)
                                {
                                    // حساب المبالغ المدفوعة والمتبقية
                                    var totalPaid = invoice.Payments.ToList();
                                    var paidAmount = totalPaid.Sum(p => p.Amount);
                                    var remainingAmount = invoice.TotalAmount - paidAmount;

                                    // إضافة صف الفاتورة في الجدول
                                    table.Cell().Element(CellStyle).Text(invoice.Customer.Name ?? "N/A");
                                    table.Cell().Element(CellStyle).Text(invoice.InvoiceNumber).AlignCenter();
                                    table.Cell().Element(CellStyle).Text(paidAmount.ToString("C")).AlignCenter();
                                    table.Cell().Element(CellStyle).Text(remainingAmount.ToString("C")).AlignCenter();
                                    table.Cell().Element(CellStyle).Text(invoice.InvoiceDate.ToString("yyyy-MM-dd")).AlignCenter();
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
}
