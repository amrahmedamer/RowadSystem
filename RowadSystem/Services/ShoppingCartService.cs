using RowadSystem.Shard.Contract.Image;
using RowadSystem.Shard.Contract.Orders;
using RowadSystem.Shard.Contract.ShoppingCarts;

namespace RowadSystem.Services;

public class ShoppingCartService(ApplicationDbContext context) : IShoppingCartService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<Result<ShoppingCart>> GetOrCreateCartAsync(ShoppingCartRequest request)
    {
        if (string.IsNullOrEmpty(request.userId) && string.IsNullOrEmpty(request.guestId))
            return Result.Failure<ShoppingCart>(ShoppingCartErrors.MissingUserOrGuest);

        var cart = await _context.ShoppingCarts
            .Include(c => c.shoppingCartItems)
            .FirstOrDefaultAsync(c =>
                c.Status == ShoppingCartStatus.Pending &&
                ((request.userId != null && c.UserId == request.userId) || (request.guestId != null && c.GuestId == request.guestId)));

        if (cart != null)
            return Result.Success(cart);

        var newCart = new ShoppingCart
        {
            UserId = request.userId,
            GuestId = request.guestId,
            Status = ShoppingCartStatus.Pending
        };

        _context.ShoppingCarts.Add(newCart);
        await _context.SaveChangesAsync();
        return Result.Success(newCart);
    }

    public async Task<Result> AddItemToCartAsync(int cartId, ShoppingCartItemRequest request)
    {
        var cart = await _context.ShoppingCarts
            .Include(c => c.shoppingCartItems)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart is null)
            return Result.Failure(ShoppingCartErrors.NotFound);


        var item = request.Adapt<ShoppingCartItem>();

        var product = await _context.Products
              .Include(p => p.productUnits)
              .Include(i => i.Inventory)
              .FirstOrDefaultAsync(p => p.Id == item.ProductId);

        if (product is null)
            return Result.Failure(ProductErrors.ProductNotFound);

        var unit = product!.productUnits.FirstOrDefault(x => x.UnitId == item.UnitId);

        if (unit is null)
            return Result.Failure(ProductErrors.UnitNotFound);

        if (product.Inventory is null)
            return Result.Failure(ProductErrors.MissingInventory);

        if (product.Inventory!.Quantity < 0 && product.Inventory.Quantity < item.Quantity)
            return Result.Failure(ProductErrors.InvalidQuantity);


        var ShoppingCartItem = new ShoppingCartItem
        {
            ShoppingCartId = cartId,
            ProductId = product.Id,
            UnitId = unit.UnitId,
            Quantity = item.Quantity,
            Price = unit.SellingPrice,
            Total = unit.SellingPrice * item.Quantity,
        };

        var existingItem = cart!.shoppingCartItems
            .FirstOrDefault(i => i.ProductId == ShoppingCartItem.ProductId && i.UnitId == ShoppingCartItem.UnitId);

        if (existingItem != null)
        {
            existingItem.Quantity += ShoppingCartItem.Quantity;
            existingItem.Total += ShoppingCartItem.Total;
        }
        else
        {
            cart.shoppingCartItems.Add(ShoppingCartItem);
        }

        cart.TotalAmount = cart.shoppingCartItems.Sum(i => i.Total);

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<IEnumerable<ShoppingCartResponse>>> GetCartItemsAsync(ShoppingCartRequest request)
    {
        var items = await _context.ShoppingCarts
            .Where(c =>
                c.Status == ShoppingCartStatus.Pending &&
                ((request.userId != null && c.UserId == request.userId) || (request.guestId != null && c.GuestId == request.guestId)))
            .Include(c => c.shoppingCartItems)
            .Select(x =>
                 x.shoppingCartItems.Select(s => new ShoppingCartResponse
                 {
                     Id = s.Id,
                     ProductId = s.ProductId,
                     ProductName = s.Product.Name,
                     Quantity = s.Quantity,
                     UnitId = s.UnitId,
                     Price = s.Price,
                     Total = s.Total,
                     TotalShoppingCart = x.TotalAmount,
                     ImageResponse = s.Product.Images!.Select(x => new ImageResponse { ImageUrL = x.ImageUrl, ThumbnailUrl = x.ThumbnailUrl }).ToList()

                 }
            )).FirstOrDefaultAsync();


        if (items is null)
            return Result.Failure<IEnumerable<ShoppingCartResponse>>(ShoppingCartErrors.ItemNotFound);

        return Result.Success(items);
    }
    public async Task<Result<CheckoutResponse>> CheckoutCartAsync(ShoppingCartRequest request, OrderRequest orderRequest)
    {
        if (string.IsNullOrEmpty(request.userId) && string.IsNullOrEmpty(request.guestId))
            return Result.Failure< CheckoutResponse>(ShoppingCartErrors.MissingUserOrGuest);

        if (string.IsNullOrEmpty(request.userId))
            return Result.Failure< CheckoutResponse>(ShoppingCartErrors.MustBeLoggedInToCheckout);

        var cart = await _context.ShoppingCarts
            .Include(c => c.shoppingCartItems)
            .FirstOrDefaultAsync(c => c.Status == ShoppingCartStatus.Pending &&
                ((request.userId != null && c.UserId == request.userId) || (request.guestId != null && c.GuestId == request.guestId)));

        if (cart is not null)
        {
            cart.UserId = request.userId;
            cart.GuestId = request.guestId;
            await _context.SaveChangesAsync();
        }


        if (cart is null || !cart.shoppingCartItems.Any())
            return Result.Failure<CheckoutResponse>(ShoppingCartErrors.EmptyCart);


        var user = await _context.Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(u => u.Id == request.userId);



        _context.Update(user);
        await _context.SaveChangesAsync();


        if (user is null)
            Result.Failure(UserErrors.UserNotFound);


        var address = new Address
        {
            GovernorateId = orderRequest.Address.GovernorateId,
            CityId = orderRequest.Address.CityId,
            Street = orderRequest.Address.Street,
            AddressDetails = orderRequest.Address.AddressDetails
        };



        var order = new Order
        {
            OrderDate = DateTime.Now,
            UserId = cart.UserId,
            Address = address,
            TotalAmount = cart.shoppingCartItems.Sum(i => i.Total),
            OrderItems = cart.shoppingCartItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitId = i.UnitId,
                Price = i.Price,
                Total = i.Price * i.Quantity
            }).ToList()
             
        };


        cart.Status = ShoppingCartStatus.CheckedOut;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var response = new CheckoutResponse
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount
        };

        return Result.Success(response);
    }
    //public async Task<Result> CheckoutCartAsync(ShoppingCartRequest request, AddressRequest address)
    //{
    //    if (string.IsNullOrEmpty(request.userId) && string.IsNullOrEmpty(request.guestId))
    //        return Result.Failure(ShoppingCartErrors.MissingUserOrGuest);

    //    if (string.IsNullOrEmpty(request.userId))
    //        return Result.Failure(ShoppingCartErrors.MustBeLoggedInToCheckout);

    //    var cart = await _context.ShoppingCarts
    //        .Include(c => c.shoppingCartItems)
    //        .FirstOrDefaultAsync(c => c.Status == ShoppingCartStatus.Pending &&
    //            ((request.userId != null && c.UserId == request.userId) || (request.guestId != null && c.GuestId == request.guestId)));

    //    if (cart is not null)
    //    {
    //        cart.UserId = request.userId;
    //        cart.GuestId = request.guestId;
    //        await _context.SaveChangesAsync();
    //    }


    //    if (cart is null || !cart.shoppingCartItems.Any())
    //        return Result.Failure(ShoppingCartErrors.EmptyCart);


    //    var user = await _context.Users
    //        .Include(x => x.Address)
    //        .FirstOrDefaultAsync(u => u.Id == request.userId);

    //    address.Adapt(user!.Address);

    //    _context.Update(user);
    //    await _context.SaveChangesAsync();


    //    if (user is null)
    //        Result.Failure(UserErrors.UserNotFound);



    //    var order = new Order
    //    {
    //        OrderDate = DateTime.Now,
    //        UserId = cart.UserId,
    //        Address = user!.Address,
    //        TotalAmount = cart.shoppingCartItems.Sum(i => i.Total),
    //        OrderItems = cart.shoppingCartItems.Select(i => new OrderItem
    //        {
    //            ProductId = i.ProductId,
    //            Quantity = i.Quantity,
    //            UnitId = i.UnitId,
    //            Price = i.Price,
    //            Total = i.Price * i.Quantity
    //        }).ToList()
    //    };

    //    cart.Status = ShoppingCartStatus.CheckedOut;
    //    _context.Orders.Add(order);
    //    await _context.SaveChangesAsync();

    //    return Result.Success();
    //}

    public async Task<Result> ClearCartAsync(int cartId)
    {
        var cart = await _context
            .ShoppingCarts
            .Where(i => i.Id == cartId).SingleOrDefaultAsync();

        if (cart is null)
            return Result.Failure(ShoppingCartErrors.NotFound);

        _context.ShoppingCarts.Remove(cart);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
    public async Task<Result> RemoveItemFromCartAsync(ShoppingCartRequest request, int itemId)
    {
        if (string.IsNullOrEmpty(request.userId) && string.IsNullOrEmpty(request.guestId))
            return Result.Failure(ShoppingCartErrors.MissingUserOrGuest);

        var cart = await _context
             .ShoppingCarts
             .Where(s => s.Status == ShoppingCartStatus.Pending && (s.UserId == request.userId || s.GuestId == request.guestId))
             .Include(s => s.shoppingCartItems)
             .FirstOrDefaultAsync();

        if (cart == null)
            return Result.Failure(ShoppingCartErrors.NotFound);

        var item = cart.shoppingCartItems.FirstOrDefault(i => i.Id == itemId);

        if (item is null)
            return Result.Failure(ShoppingCartErrors.ItemNotFound);


        _context.ShoppingCartItems.Remove(item);
        await _context.SaveChangesAsync();

        cart.TotalAmount = cart.shoppingCartItems.Sum(x => x.Total);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    //public async Task<Result> UpdateItemQuantityAsync(int cartId, int itemId, int quantity)
    //{
    //    var item = await _context.ShoppingCarts
    //        .Where(c => c.Status == ShoppingCartStatus.Pending && c.Id == cartId)
    //        .SelectMany(cart => cart.shoppingCartItems.Where(i => i.Id == itemId))
    //       .Include(p => p.Product)
    //       .FirstOrDefaultAsync();


    //    var quantityCheck = item.Product.Inventory.Quantity;
    //    if (quantityCheck < quantity)
    //        return Result.Failure(ProductErrors.InvalidQuantity);

    //    item.Quantity = quantity;
    //    await _context.SaveChangesAsync();

    //    return Result.Success();
    //}


    public async Task<Result> UpdateItemQuantityAsync(int cartId, int itemId, int quantity)
    {
        var cartData = await _context.ShoppingCarts
            .Where(c => c.Status == ShoppingCartStatus.Pending && c.Id == cartId)
            .Include(x => x.shoppingCartItems)
            .ThenInclude(i => i.Product)
            .ThenInclude(x => x.Inventory)
            .FirstOrDefaultAsync();

        if (cartData == null)
            return Result.Failure(ShoppingCartErrors.NotFound);

        var item = cartData.shoppingCartItems
       .FirstOrDefault(i => i.Id == itemId);

        if (item == null)
            return Result.Failure(ShoppingCartErrors.ItemNotFound);

        if (item.Product == null || item.Product.Inventory == null)
            return Result.Failure(ProductErrors.InvalidQuantity);

        if (item.Product.Inventory.Quantity < quantity)
            return Result.Failure(ProductErrors.InvalidQuantity);



        item.Quantity = quantity;
        item.Total = item.Price * item.Quantity;

        cartData.TotalAmount = cartData.shoppingCartItems.Sum(i => i.Total);

        _context.ShoppingCartItems.Update(item);

        _context.ShoppingCarts.Update(cartData);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
    public async Task<Result<int>> CountCartItem(int cartId)
    {
        var count = await _context.ShoppingCarts
            .Where(c => c.Status == ShoppingCartStatus.Pending && c.Id == cartId)
            .Include(x => x.shoppingCartItems)
            .Select(
            x => x.shoppingCartItems.Count
            )
            .FirstOrDefaultAsync();


        return Result.Success(count);
    }


}
