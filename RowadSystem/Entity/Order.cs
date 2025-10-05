using RowadSystem.API.Entity;

namespace RowadSystem.Entity;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string? UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public ApplicationUser? User { get; set; }
    public int? AddressId { get; set; }
    public Address Address { get; set; } = null!;
    public SalesInvoice? Invoice { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; } = 0;
    public PaymentMethodOrder PaymentMethod { get; set; }
    public string? PaymentTransactionId { get; set; }


}
