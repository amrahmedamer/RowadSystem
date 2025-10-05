using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Services;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountStatementsController(IAccountStatementService accountStatementService) : ControllerBase
{
    private readonly IAccountStatementService _accountStatementService = accountStatementService;

    [HttpGet("sales/summary")]
    public async Task<IActionResult> GetSalesSummary()
    {
        var result = await _accountStatementService.GetSalesSummary();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("sales/details")]
    public async Task<IActionResult> GetSalesDetails( [FromQuery]RequestFilters request)
    {
        var result = await _accountStatementService.GetSalesDetails(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    //[HttpGet("sales/details")]
    //public async Task<IActionResult> GetSalesDetails()
    //{
    //    var result = await _accountStatementService.GetSalesDetails();

    //    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    //}
    [HttpGet("purchase/summary")] 
    public async Task<IActionResult> GetPurchaseSummary()
    {
        var result = await _accountStatementService.GetPurchaseSummary();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("purchase/details")]
    public async Task<IActionResult> GetPurchaseDetails([FromQuery] RequestFilters request)
    {
        var result = await _accountStatementService.GetPurchaseDetails(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("sales-return/summary")]
    public async Task<IActionResult> GetSalesReturnSummary()
    {
        var result = await _accountStatementService.GetSalesReturnSummary();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("sales-return/details")] 
    public async Task<IActionResult> GetSalesReturnDetails([FromQuery] RequestFilters request)
    {
        var result = await _accountStatementService.GetSalesReturnDetails(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("purchase-return/summary")] 
    public async Task<IActionResult> GetPurchaseReturnSummary()
    {
        var result = await _accountStatementService.GetPurchaseReturnSummary();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("purchase-return/details")]
    public async Task<IActionResult> GetPurchaseReturnDetails([FromQuery] RequestFilters request)
    {
        var result = await _accountStatementService.GetPurchaseReturnDetails(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}

