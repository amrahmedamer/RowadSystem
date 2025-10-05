using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Orders;

namespace RowadSystem.Services;
public class OrderService(ApplicationDbContext context) : IOrderService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<OrderResponse>> GetOrderByUserIdsAsync(string userId)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId && o.Status == OrderStatus.Pending)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Address)
            .Select(o => new OrderResponse()
            {
                Id = o.Id,
                UserId = o.UserId!,
                UserName = o.User!.UserName!,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                OrderItems = o.OrderItems.Select(oi => new OrderItemResponse

                {
                    Id = oi.Id,
                    ProductName = oi.Product!.Name,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitId = oi.UnitId,
                    Price = oi.Price,
                    Total = oi.Total
                }).ToList(),
                Address = new AddressResponse
                {
                    GovernorateName = o.Address!.Governorate.Name,
                    CityName = o.Address.City.Name,
                    Street = o.Address.Street,
                    AddressDetails = o.Address.AddressDetails,
                }
            })
            .OrderByDescending(o => o.OrderDate)
            .FirstOrDefaultAsync();

        if (orders is null)
            return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);

        return Result.Success(orders);
    }
    public async Task<Result> UpdatePaymentMethodByUserIdsAsync(string userId, PaymentMethodOrder paymentMethodOrder)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId && o.Status == OrderStatus.Pending)
            .OrderByDescending(o => o.OrderDate)
            .FirstOrDefaultAsync();


        if (orders is null)
            return Result.Failure(OrderErrors.OrderNotFound);

        orders.PaymentMethod = paymentMethodOrder;
        await _context.SaveChangesAsync();

        return Result.Success();
    }
    public async Task<Result<List<OrderResponse>>> GetAllOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Address)
            .ToListAsync();

        if (orders.Count == 0)
            return Result.Failure<List<OrderResponse>>(OrderErrors.OrderNotFound);

        var orderResponses = orders.Adapt<List<OrderResponse>>();
        return Result.Success(orderResponses);
    }
    public async Task<Result> AddOrderAsync(string UserId, List<OrderItemRequest> request)
    {
        //if (await _context.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == UserId) is not { } user)
        //    return Result.Failure(OrderErrors.MustBeLoggedInToCreateOrder);

        if (UserId is not null)
        {
            if (await _context.Orders.AnyAsync(x => x.UserId == UserId
               && (x.Status == OrderStatus.Pending)))
                return Result.Failure(OrderErrors.UserAlreadyHasOrder);
        }

        var orderlist = request.Adapt<List<OrderItem>>();
        var productIds = request.Select(x => x.ProductId).ToList();

        if (!productIds.Any())
            return Result.Failure(OrderErrors.OrderItemsCannotBeEmpty);

        var products = await _context.Products
            .Where(x => productIds.Contains(x.Id))
            .Include(x => x.productUnits)
            .Include(x => x.Inventory)
            .ToListAsync();

        if (products.Count != productIds.Count)
            return Result.Failure(OrderErrors.SomeProductsNotFound);

        var orderItems = new List<OrderItem>();
        foreach (var item in orderlist)
        {
            var product = products.SingleOrDefault(x => x.Id == item.ProductId);

            if (product == null)
                return Result.Failure(ProductErrors.ProductNotFound);

            if (product.productUnits is null || !product.productUnits.Any())
                return Result.Failure(ProductErrors.ProductUnitsNotFound);


            var productUnits = product.productUnits.Where(p => p.UnitId == item.UnitId).ToList();

            if (!productUnits.Any())
                return Result.Failure(ProductErrors.UnitNotFound);

            if (product.Inventory is null || product.Inventory.Quantity < item.Quantity || item.Quantity < 0)
                return Result.Failure(ProductErrors.ProductQuantityNotAvailable);

            var price = product.productUnits.Where(p => p.UnitId == item.UnitId)
                              .Select(p => p.SellingPrice).SingleOrDefault();

            orderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = price,
                UnitId = item.UnitId,
                Total = item.Quantity * price
            });
        }

        //if (user.Address is null)
        //    return Result.Failure(UserErrors.AddressRequired);

        var order = new Order
        {
            UserId = UserId,
            //AddressId = user.Address.Id,
            TotalAmount = orderItems.Sum(x => x.Total),
            OrderItems = orderItems
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
    public async Task<Result> UpdateOrderAsync(int orderId, string userId, List<OrderItemRequest> request)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.User)
            //.ThenInclude(u => u.Address)
            .SingleOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null)
            return Result.Failure(OrderErrors.OrderNotFound);

        if (order.Status != OrderStatus.Pending)
            return Result.Failure(OrderErrors.CannotUpdateOrder);

        var productIds = request.Select(x => x.ProductId).ToList();

        if (!productIds.Any())
            return Result.Failure(OrderErrors.OrderItemsCannotBeEmpty);

        var products = await _context.Products
            .Where(x => productIds.Contains(x.Id))
            .Include(x => x.productUnits)
            .Include(x => x.Inventory)
            .ToListAsync();

        if (products.Count != productIds.Count)
            return Result.Failure(OrderErrors.SomeProductsNotFound);

        var updatedItems = new List<OrderItem>();
        foreach (var item in request)
        {
            var product = products.SingleOrDefault(x => x.Id == item.ProductId);
            if (product == null)
                return Result.Failure(ProductErrors.ProductNotFound);

            if (product.productUnits is null || !product.productUnits.Any())
                return Result.Failure(ProductErrors.ProductUnitsNotFound);

            var productUnit = product.productUnits.FirstOrDefault(p => p.UnitId == item.UnitId);
            if (productUnit == null)
                return Result.Failure(ProductErrors.UnitNotFound);

            if (product.Inventory == null || product.Inventory.Quantity < item.Quantity || item.Quantity < 0)
                return Result.Failure(ProductErrors.ProductQuantityNotAvailable);

            updatedItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitId = item.UnitId,
                Price = productUnit.SellingPrice,
                Total = productUnit.SellingPrice * item.Quantity,
                OrderId = order.Id
            });
        }

        //if (order.User!.Address is null)
        //    return Result.Failure(UserErrors.AddressRequired);

        _context.OrderItems.RemoveRange(order.OrderItems);
        order.OrderItems = updatedItems;
        order.TotalAmount = updatedItems.Sum(x => x.Total);

        await _context.SaveChangesAsync();
        return Result.Success();
    }

}
