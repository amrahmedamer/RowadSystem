using RowadSystem.Shard.Contract.Orders;

namespace RowadSystem.Services;

public interface IOrderService
{
    Task<Result> AddOrderAsync(string UserId, List<OrderItemRequest> request);
    Task<Result> UpdateOrderAsync(int orderId, string userId, List<OrderItemRequest> request);
    Task<Result<OrderResponse>> GetOrderByUserIdsAsync(string userId);
    Task<Result<List<OrderResponse>>> GetAllOrdersAsync();
    Task<Result> UpdatePaymentMethodByUserIdsAsync(string userId, PaymentMethodOrder paymentMethodOrder);
}
