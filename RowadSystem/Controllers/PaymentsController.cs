using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Paymob;

namespace RowadSystem.Controllers;
[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class PaymentsController (IPaymentService paymentService, IPaymobService paymobService) : ControllerBase
{
    private readonly IPaymentService _paymentService = paymentService;
    private readonly IPaymobService _paymobService = paymobService;


    [HttpPost("card")]
    public async Task<IActionResult> StartCard([FromBody] StartPaymentRequest req, CancellationToken ct)
    {
        if (req == null)
        {
            return BadRequest("Payment request cannot be null.");
        }

        try
        {
            var res = await _paymobService.StartCardPaymentAsync(req, ct);
            return Ok(res);
        }
        catch (Exception ex)
        {
            // سجل الاستثناء
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An error occurred while processing the payment.");
        }
    }

    [HttpPost("wallet")]
    public async Task<IActionResult> StartWallet([FromBody] StartWalletPaymentRequest req, CancellationToken ct)
    {
        if (req == null)
        {
            return BadRequest("Request cannot be null");
        }
        var res = await _paymobService.StartWalletPaymentAsync(req, ct);
        return Ok(res);
    }

    // Paymob webhook
    [HttpPost("webhook")]
    public IActionResult Webhook([FromBody] PaymobWebhookPayload payload)
    {
        if (!_paymobService.VerifyHmac(payload))
            return BadRequest("Invalid HMAC");

        // TODO: Update order status in DB using payload.order.id and payload.success
        return Ok();
    }
}

