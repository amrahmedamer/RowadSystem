using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Address;

namespace RowadSystem.Shard.Contract.Orders;

public class OrderResponse
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public AddressResponse Address { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemResponse> OrderItems { get; set; }
};
