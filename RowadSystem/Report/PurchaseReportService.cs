using QuestPDF.Fluent;
using QuestPDF.Helpers;
using RowadSystem.API.Report;
using QuestPDF.Infrastructure;

public class PurchaseReportService : IPurchaseReportService
{
    private readonly ApplicationDbContext _context;

    public PurchaseReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public byte[] GeneratePurchaseReportPdf()
    {
        var purchaseData = _context.PurchaseInvoices
            .Include(pi => pi.purchaseInvoiceDetails)
            .ThenInclude(pid => pid.Product)
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
                            .Text("Purchase Report")
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
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Product Name").AlignStart();
                                    header.Cell().Element(CellStyle).Text("Quantity Purchased").AlignCenter();
                                    header.Cell().Element(CellStyle).Text("Unit Price").AlignCenter();
                                });

                                foreach (var purchase in purchaseData)
                                {
                                    foreach (var detail in purchase.purchaseInvoiceDetails)
                                    {
                                        table.Cell().Element(CellStyle).Text(detail.Product.Name);
                                        table.Cell().Element(CellStyle).Text(detail.Quantity.ToString()).AlignCenter();
                                        table.Cell().Element(CellStyle).Text(detail.Price.ToString("C")).AlignCenter();
                                    }
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
        return container.BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).PaddingVertical(5);
    }
}
