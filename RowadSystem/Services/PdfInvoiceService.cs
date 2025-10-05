//using QuestPDF.Fluent;
//using QuestPDF.Helpers;
//using QuestPDF.Infrastructure;

//namespace RowadSystem.API.Services;



//public class PdfInvoiceService : IPdfInvoiceService
//{
//    public byte[] GenerateInvoice(SalesInvoiceResponse invoice)
//    {
//        var document = Document.Create(container =>
//        {
//            container.Page(page =>
//            {
//                page.Margin(30);
//                page.Size(PageSizes.A4);

//                // Header
//                page.Header().Row(row =>
//                {
//                    row.RelativeColumn().Text($"Invoice #{invoice.SalesInvoiceId}")
//                        .FontSize(20).Bold();
//                    row.ConstantColumn(100).Height(50).Image("wwwroot/Logo.png", ImageScaling.FitArea);
//                });

//                // Content
//                page.Content().Column(col =>
//                {
//                    col.Spacing(10);

//                    col.Item().Text($"Customer: {invoice.CustomerId}");
//                    col.Item().Text($"Date: {invoice.InvoiceDate:d}");

//                    col.Item().Table(table =>
//                    {
//                        table.ColumnsDefinition(columns =>
//                        {
//                            columns.RelativeColumn();
//                            columns.ConstantColumn(60);
//                            columns.ConstantColumn(80);
//                        });

//                        // Table Header
//                        table.Header(header =>
//                        {
//                            header.Cell().Text("Product").Bold();
//                            header.Cell().Text("Qty").Bold();
//                            header.Cell().Text("Price").Bold();
//                        });

//                        // Table Rows
//                        foreach (var item in invoice.Items)
//                        {
//                            table.Cell().Text(item.ProductName);
//                            table.Cell().Text(item.Quantity.ToString());
//                            table.Cell().Text($"{item.TotalPrice:C}");
//                        }
//                    });

//                    col.Item().AlignRight().Text($"Total: {2000:C}").FontSize(16).Bold();
//                });

//                // Footer
//                page.Footer().AlignCenter().Text("Thank you for your purchase!");
//            });
//        });

//        return document.GeneratePdf();
//    }
//}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RowadSystem.Entity;

namespace RowadSystem.API.Services;

public class PdfInvoiceService : IPdfInvoiceService
{
    //[Obsolete]
    //public byte[] GenerateInvoice(SalesInvoiceResponse invoice)
    //{
    //    var document = Document.Create(container =>
    //    {
    //        container.Page(page =>
    //        {
    //            page.Margin(30);
    //            page.Size(PageSizes.A6);

    //            // Header
    //            page.Header().Row(row =>
    //            {
    //                row.RelativeColumn().Stack(stack =>
    //                {
    //                    stack.Item().Text("Rowad System")
    //                        .Bold().FontSize(18);
    //                    stack.Item().Text($"Date: {invoice.InvoiceDate:yyyy/MM/dd}");
    //                    stack.Item().Text($"Invoice Number : {invoice.InvoiceNumber}");
    //                    stack.Item().Text(invoice.UserId is null ? $"Customer : {invoice.CustomerName}":
    //                        $"User : {invoice.UserName}")
    //                        .FontColor(Colors.Grey.Darken2);
    //                });

    //                //row.RelativeColumn().Text($"Invoice #{invoice.SalesInvoiceId}")
    //                //    .FontSize(20).Bold();

    //                //// ✅ الطريقة الجديدة لعرض الصورة
    //                //row.ConstantColumn(50)
    //                //    .Height(50)
    //                //    .Image("wwwroot/LogoInvoice.png")
    //                //    .FitArea();
    //            });

    //            // Content
    //            page.Content().Column(col =>
    //            {
    //                col.Spacing(10);

    //                //col.Item().Text($"Customer: {invoice.CustomerId}");
    //                //col.Item().Text($"Date: {invoice.InvoiceDate:d}");

    //                col.Item().Table(table =>
    //                {
    //                    table.ColumnsDefinition(columns =>
    //                    {
    //                        columns.RelativeColumn();
    //                        columns.ConstantColumn(60);
    //                        columns.ConstantColumn(80);
    //                    });

    //                    // Table Header
    //                    table.Header(header =>
    //                    {
    //                        header.Cell().Text("Product").Bold();
    //                        header.Cell().Text("Qty").Bold();
    //                        header.Cell().Text("Price").Bold();
    //                    });

    //                    // Table Rows
    //                    foreach (var item in invoice.Items)
    //                    {
    //                        table.Cell().Text(item.ProductName);
    //                        table.Cell().Text(item.Quantity.ToString());
    //                        table.Cell().Text($"{item.TotalPrice:C}");
    //                    }
    //                });

    //                col.Item().AlignRight()
    //                    .Text($"Total: {2000:C}")
    //                    .FontSize(16)
    //                    .Bold();
    //            });

    //            // Footer
    //            page.Footer()
    //                .AlignCenter()
    //                .Text("Thank you for your purchase!");
    //        });
    //    });

    //    return document.GeneratePdf();
    //}

    [Obsolete]
    public byte[] GenerateInvoice(SalesInvoiceResponse invoice)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A6);


                // Header
                page.Header().Row(row =>
                {
                    row.RelativeColumn().Stack(stack =>
                    {
                        stack.Item().Text("Rowad System").Bold().FontSize(18);
                        stack.Item().Text($"Date: {invoice.InvoiceDate:yyyy/MM/dd}");
                        stack.Item().Text($"Invoice No: {invoice.InvoiceNumber}");
                        stack.Item().Text(invoice.UserId is null
                            ? $"Customer: {invoice.CustomerName ?? null}"
                            : $"User: {invoice.UserName}")
                            .FontColor(Colors.Grey.Darken2);
                    });

                    // Optional Logo
                    //row.ConstantColumn(50)
                    //   .Height(50)
                    //   .Image("wwwroot/LogoInvoice.png")
                    //   .FitArea();
                });

                // Content
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();  // Product
                            columns.ConstantColumn(40); // Qty
                            columns.ConstantColumn(60); // Price
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Text("Product").Bold();
                            header.Cell().Text("Qty").Bold();
                            header.Cell().Text("Price").Bold();
                        });
                        if (invoice.Items != null)
                        {
                            // Rows
                            foreach (var item in invoice.Items)
                            {
                                table.Cell().Text(item.ProductName ?? "-----");
                                table.Cell().AlignCenter().Text(item.Quantity.ToString() ?? "0");
                                table.Cell().AlignRight().Text($"{item.TotalPrice:C}");
                            }
                        }
                        else
                        {
                            col.Item().Text("No items found");
                        }
                    });

                    // Total
                    var total = invoice.Items.Sum(i => i.TotalPrice);
                    col.Item().AlignRight()
                        .Text($"Total: {total:C}")
                        .FontSize(14)
                        .Bold();
                });

                // Footer
                page.Footer().AlignCenter().Text("Thank you for your purchase!");
            });
        });

        return document.GeneratePdf();
    }


    [Obsolete]
    public byte[] GenerateInvoice(PurchaseInvoiceResponse invoice)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A6);

                // Header
                page.Header().Row(row =>
                {
                    row.RelativeColumn().Stack(stack =>
                    {
                        stack.Item().Text("Rowad System")
                            .Bold().FontSize(18);
                        stack.Item().Text($"Date: {invoice.InvoiceDate:yyyy/MM/dd}");
                        stack.Item().Text($"Invoice Number : {invoice.InvoiceNumber}");
                        stack.Item().Text($"Supplier : {invoice.SupplierName}")
                            .FontColor(Colors.Grey.Darken2);
                    });

                    //row.RelativeColumn().Text($"Invoice #{invoice.Inv}")
                    //    .FontSize(20).Bold();

                    // ✅ الطريقة الجديدة لعرض الصورة
                    //row.ConstantColumn(100)
                    //    .Height(50)
                    //    .Image("wwwroot/LogoInvoice.png")
                    //    .FitArea();
                });

                // Content
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    //col.Item().Text($"Supplier: {invoice.SupplierId}");
                    //col.Item().Text($"Date: {invoice.InvoiceDate:d}");

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(80);
                        });

                        // Table Header
                        table.Header(header =>
                        {
                            header.Cell().Text("Product").Bold();
                            header.Cell().Text("Qty").Bold();
                            header.Cell().Text("Price").Bold();
                        });

                        // Table Rows
                        foreach (var item in invoice.Items)
                        {
                            table.Cell().Text(item.ProductName);
                            table.Cell().Text(item.Quantity.ToString());
                            table.Cell().Text($"{item.TotalPrice:N2} EGP");

                        }
                    });

                    // Total
                    var total = invoice.Items.Sum(i => i.TotalPrice);
                    col.Item().AlignRight()
                        .Text($"Total: {total:N2} EGP")
                        .FontSize(14)
                        .Bold();
                });

                // Footer
                page.Footer()
                    .AlignCenter()
                    .Text("Thank you for your purchase!");
            });
        });

        return document.GeneratePdf();
    }
    [Obsolete]
    public byte[] GenerateInvoice(SalesReturnInvoiceResponse invoice)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A6);


                // Header
                page.Header().Row(row =>
                {
                    row.RelativeColumn().Stack(stack =>
                    {
                        stack.Item().Text("Rowad System").Bold().FontSize(18);
                        stack.Item().Text($"Date: {invoice.InvoiceDate:yyyy/MM/dd}");
                        stack.Item().Text($"Invoice No: {invoice.InvoiceNumber}");
                        stack.Item().Text(invoice.UserId is null
                            ? $"Customer: {invoice.CustomerName}"
                            : $"User: {invoice.UserName}")
                            .FontColor(Colors.Grey.Darken2);
                    });

                    // Optional Logo
                    //row.ConstantColumn(50)
                    //   .Height(50)
                    //   .Image("wwwroot/LogoInvoice.png")
                    //   .FitArea();
                });

                // Content
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();  // Product
                            columns.ConstantColumn(40); // Qty
                            columns.ConstantColumn(60); // Price
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Text("Product").Bold();
                            header.Cell().Text("Qty").Bold();
                            header.Cell().Text("Price").Bold();
                        });

                        // Rows
                        foreach (var item in invoice.Items)
                        {
                            table.Cell().Text(item.ProductName);
                            table.Cell().AlignCenter().Text(item.Quantity.ToString());
                            table.Cell().AlignRight().Text($"{item.TotalPrice:C}");
                        }
                    });

                    // Total
                    var total = invoice.Items.Sum(i => i.TotalPrice);
                    col.Item().AlignRight()
                        .Text($"Total: {total:C}")
                        .FontSize(14)
                        .Bold();
                });

                // Footer
                page.Footer().AlignCenter().Text("Thank you for your purchase!");
            });
        });

        return document.GeneratePdf();
    }


    [Obsolete]
    public byte[] GenerateInvoice(PurchaseReturnInvoiceResponse invoice)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A6);

                // Header
                page.Header().Row(row =>
                {
                    row.RelativeColumn().Stack(stack =>
                    {
                        stack.Item().Text("Rowad System")
                            .Bold().FontSize(18);
                        stack.Item().Text($"Date: {invoice.InvoiceDate:yyyy/MM/dd}");
                        stack.Item().Text($"Invoice Number : {invoice.InvoiceNumber}");
                        stack.Item().Text($"Supplier : {invoice.SupplierName}")
                            .FontColor(Colors.Grey.Darken2);
                    });

                    //row.RelativeColumn().Text($"Invoice #{invoice.Inv}")
                    //    .FontSize(20).Bold();

                    // ✅ الطريقة الجديدة لعرض الصورة
                    //row.ConstantColumn(100)
                    //    .Height(50)
                    //    .Image("wwwroot/LogoInvoice.png")
                    //    .FitArea();
                });

                // Content
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    //col.Item().Text($"Supplier: {invoice.SupplierId}");
                    //col.Item().Text($"Date: {invoice.InvoiceDate:d}");

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(80);
                        });

                        // Table Header
                        table.Header(header =>
                        {
                            header.Cell().Text("Product").Bold();
                            header.Cell().Text("Qty").Bold();
                            header.Cell().Text("Price").Bold();
                        });

                        // Table Rows
                        foreach (var item in invoice.Items)
                        {
                            table.Cell().Text(item.ProductName);
                            table.Cell().Text(item.Quantity.ToString());
                            table.Cell().Text($"{item.TotalPrice:N2} EGP");

                        }
                    });

                    // Total
                    var total = invoice.Items.Sum(i => i.TotalPrice);
                    col.Item().AlignRight()
                        .Text($"Total: {total:N2} EGP")
                        .FontSize(14)
                        .Bold();
                });

                // Footer
                page.Footer()
                    .AlignCenter()
                    .Text("Thank you for your purchase!");
            });
        });

        return document.GeneratePdf();
    }
}
