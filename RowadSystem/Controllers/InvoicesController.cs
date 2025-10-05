using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RowadSystem.API.Services;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class InvoicesController(IInvoicesService invoicesService, IPdfInvoiceService pdfInvoice) : ControllerBase
{
    private readonly IInvoicesService _invoicesService = invoicesService;
    private readonly IPdfInvoiceService _pdfInvoice = pdfInvoice;

    [HttpGet("sales/{InvoiceNumber}")]
    public async Task<IActionResult> GetSalesInvoice([FromRoute] string InvoiceNumber)
    {
        var result = await _invoicesService.GetSalesInvoicesByInvoiceNumber(InvoiceNumber);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("purchase/{InvoiceNumber}")]
    public async Task<IActionResult> GetPurchaseInvoice([FromRoute] string InvoiceNumber)
    {
        var result = await _invoicesService.GetPurchaseInvoicesByInvoiceNumber(InvoiceNumber);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("sales-return/{InvoiceNumber}")]
    public async Task<IActionResult> GetSalesReturnInvoice([FromRoute] string InvoiceNumber)
    {
        var result = await _invoicesService.GetSalesReturnInvoicesByInvoiceNumber(InvoiceNumber);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("purchase-return/{InvoiceNumber}")]
    public async Task<IActionResult> GetPurchaseReturnInvoice([FromRoute] string InvoiceNumber)
    {
        var result = await _invoicesService.GetPurchaseReturnInvoicesByInvoiceNumber(InvoiceNumber);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("sales")]
    public async Task<IActionResult> AddSalesInvoice([FromBody] SalesInvoiceRequest request)
    {
        var result = await _invoicesService.AddSalesInvoiceAsync(User.GetUserId(), request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("sales-return")]
    public async Task<IActionResult> AddSalesReturnInvoice([FromBody] SalesReturnInvoiceRequest request)
    {
        var result = await _invoicesService.AddSalesReturnInvoiceAsync(User.GetUserId(), request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("purchase")]
    public async Task<IActionResult> AddPurchaseInvoice([FromBody] PurchaseInvoiceRequest request)
    {
        var result = await _invoicesService.AddPurchaseInvoiceAsync(User.GetUserId(), request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("purchase-return")]
    public async Task<IActionResult> AddPurchaseReturnInvoice([FromBody] PurchaseReturnInvoiceRequest request)
    {
        var result = await _invoicesService.AddPurchaseReturnInvoiceAsync(User.GetUserId(), request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("print-sales/{InvoiceNumber}")]
    [AllowAnonymous]
    public async Task<IActionResult> SalesPdf([FromRoute] string InvoiceNumber)
    {

       var invoice = await _invoicesService.GetSalesInvoicesByInvoiceNumber(InvoiceNumber);
        var pdf =  _pdfInvoice.GenerateInvoice(invoice.Value);

        return File(pdf, "application/pdf");
        //return File(pdf, "application/pdf", $"Invoice_{InvoiceNumber}.pdf");
    }
    [HttpGet("print-sales-return/{InvoiceNumber}")]
    public async Task<IActionResult> SalesReturnPdf([FromRoute] string InvoiceNumber)
    {

       var invoice = await _invoicesService.GetSalesReturnInvoicesByInvoiceNumber(InvoiceNumber);
        var pdf = _pdfInvoice.GenerateInvoice(invoice.Value);
        return File(pdf, "application/pdf");
    }
    [HttpGet("print-purchase/{InvoiceNumber}")]
    public async Task<IActionResult> PurchasePdf([FromRoute] string InvoiceNumber)
    {

       var invoice = await _invoicesService.GetPurchaseInvoicesByInvoiceNumber(InvoiceNumber);
        var pdf =  _pdfInvoice.GenerateInvoice(invoice.Value);

        return File(pdf, "application/pdf");
    }
    [HttpGet("print-purchase-return/{InvoiceNumber}")]
    public async Task<IActionResult> PurchaseReturnPdf([FromRoute] string InvoiceNumber)
    {

        var invoice = await _invoicesService.GetPurchaseReturnInvoicesByInvoiceNumber(InvoiceNumber);
        var pdf = _pdfInvoice.GenerateInvoice(invoice.Value);
        return File(pdf, "application/pdf");
    }
    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesInvoice([FromQuery] RequestFilters request)
    {
        var result = await _invoicesService.GetAllSalesInvoices(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("sales-return")]
    public async Task<IActionResult> GetSalesReturnInvoice([FromQuery] RequestFilters request)
    {
        var result = await _invoicesService.GetAllSalesReturnInvoices(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("purchase")]
    public async Task<IActionResult> GetPurchaseInvoice([FromQuery] RequestFilters request)
    {
        var result = await _invoicesService.GetAllPurchaseInvoices(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("purchase-return")]
    public async Task<IActionResult> GetPurchaseReturnInvoice([FromQuery] RequestFilters request)
    {
        var result = await _invoicesService.GetAllPurchaseReturnInvoices(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("{invoiceId}/update-invoice-payment")]
    public async Task<IActionResult> UpdateInvoicePaymentsAsync([FromRoute]int invoiceId,[FromBody]PaymentRequest request)
    {
        var result = await _invoicesService.AddPaymentToInvoiceAsync(invoiceId, request);

        return result.IsSuccess ? Ok(result) : result.ToProblem();
    }
}
