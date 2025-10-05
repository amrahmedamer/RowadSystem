namespace RowadSystem.API.Services;

public interface IPdfInvoiceService
{
    byte[] GenerateInvoice(SalesInvoiceResponse invoice);
    byte[] GenerateInvoice(SalesReturnInvoiceResponse invoice);
    byte[] GenerateInvoice(PurchaseInvoiceResponse invoice);
    byte[] GenerateInvoice(PurchaseReturnInvoiceResponse invoice);
}
