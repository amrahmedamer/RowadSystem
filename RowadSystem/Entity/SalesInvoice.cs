namespace RowadSystem.Entity;

public class SalesInvoice : Invoice
{
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; }

    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int? CouponId { get; set; }
    public Coupon? Coupon { get; set; }

    public int? OrderId { get; set; }
    public Order? Order { get; set; }

    public ICollection<SalesInvoiceDetail> SalesInvoiceDetails { get; set; } = [];
    public ICollection<SalesReturnInvoice> SalesReturnInvoices { get; set; } = new List<SalesReturnInvoice>();

}
