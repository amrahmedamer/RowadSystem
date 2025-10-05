using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Phones;

namespace RowadSystem.Shard.Contract.Orders;

public class OrderRequest
{
    //public int AddressId { get; set; }
    //public List<OrderItemRequest> OrderItems { get; set; }
    public AddressRequest Address { get; set; }
    public PhoneNumberRequest PhoneNumbers { get; set; }
};
