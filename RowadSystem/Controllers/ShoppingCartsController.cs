using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Orders;
using RowadSystem.Shard.Contract.ShoppingCarts;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ShoppingCartsController(IShoppingCartService shoppingCartService, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly IShoppingCartService _shoppingCartService = shoppingCartService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private string? UserId => User.GetUserId();
    private string? GuestId => Request.Cookies["GuestId"];

    [HttpPost("")]
    public async Task<IActionResult> AddItemToCartAsync([FromBody] ShoppingCartItemRequest request)
    {

        var cart = await _shoppingCartService.GetOrCreateCartAsync(new ShoppingCartRequest(UserId, GuestId));

        if (cart is null)
            return cart!.ToProblem();

        var result = await _shoppingCartService.AddItemToCartAsync(cart.Value.Id, request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetCartItems()
    {
        var result = await _shoppingCartService.GetCartItemsAsync(new ShoppingCartRequest(UserId, GuestId));
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpDelete("remove-item/{itemId}")]
    public async Task<IActionResult> RemoveItemFromCart([FromRoute] int itemId)
    {
        var result = await _shoppingCartService.RemoveItemFromCartAsync(new ShoppingCartRequest(UserId, GuestId), itemId);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPut("update-item-quantity")]
    public async Task<IActionResult> UpdateItemQuantity([FromBody] UpdateQuantityRequest request)
    {
        var cart = await _shoppingCartService.GetOrCreateCartAsync(new ShoppingCartRequest(UserId, GuestId));
        if (cart is null)
            return cart!.ToProblem();

        request.cartId = cart.Value.Id;
        var result = await _shoppingCartService.UpdateItemQuantityAsync(request.cartId, request.itemId, request.quantity);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }


    [HttpPost("clear-cart/{cartId}")]
    public async Task<IActionResult> ClearCartAsync([FromRoute] int cartId)
    {

        var result = await _shoppingCartService.ClearCartAsync(cartId);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("count")]
    public async Task<IActionResult> CountCartItemAsync()
    {
        var cart = await _shoppingCartService.GetOrCreateCartAsync(new ShoppingCartRequest(UserId, GuestId));

        if (cart is null)
            return cart!.ToProblem();

        var result = await _shoppingCartService.CountCartItem(cart.Value.Id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpPost("check-out")]
    public async Task<IActionResult> CheckOut([FromBody] OrderRequest orderRequest)
    {
        var result = await _shoppingCartService.CheckoutCartAsync(new ShoppingCartRequest(UserId, GuestId), orderRequest);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    ////  [Authorize]
    //[HttpPost("check-out")]
    //public async Task<IActionResult> CheckOut([FromBody] AddressRequest address)
    //{
    //    var result = await _shoppingCartService.CheckoutCartAsync(new ShoppingCartRequest(UserId, GuestId), address);

    //    return result.IsSuccess ? Ok() : result.ToProblem();
    //}

}
