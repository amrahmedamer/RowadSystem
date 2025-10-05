using RowadSystem.Shard.consts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowadSystem.Shard.Contract.Orders;
public class OrderDTO
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string UserId { get; set; }  // مرتبط بـ الـ User
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public int AddressId { get; set; }  // مرتبط بعنوان العميل
    public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; } = 0;
    public PaymentMethodOrder PaymentMethod { get; set; }
    public string? PaymentTransactionId { get; set; }
}
