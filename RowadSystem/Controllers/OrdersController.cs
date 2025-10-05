using Microsoft.AspNetCore.Mvc;
using RowadSystem.Services;
using RowadSystem.Shard.Contract.Orders;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;



    [HttpPost("")]
    public async Task<IActionResult> AddOrderAsync([FromBody] List<OrderItemRequest> request)
    {
        var user = User.GetUserId();

        //var response=

        var result = await _orderService.AddOrderAsync(User.GetUserId(), request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPut("{orederId}")]
    public async Task<IActionResult> UpdateOrderAsync(int orederId, [FromBody] List<OrderItemRequest> request)
    {
        var result = await _orderService.UpdateOrderAsync(orederId, User.GetUserId(), request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("")]
    public async Task<IActionResult> UpdatePaymentMethodAsync([FromBody] PaymentMethodOrder request)
    {
        var result = await _orderService.UpdatePaymentMethodByUserIdsAsync(User.GetUserId(), request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetOrder()
    {
        var result = await _orderService.GetOrderByUserIdsAsync(User.GetUserId());
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("get-all-order")]
    public async Task<IActionResult> GetAllOrder()
    {
        var result = await _orderService.GetAllOrdersAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
