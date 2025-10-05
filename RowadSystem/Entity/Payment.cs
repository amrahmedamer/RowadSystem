namespace RowadSystem.Entity;

public class Payment
{
    public int Id { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public PaymentMethod Method { get; set; } = PaymentMethod.Manual;
    public int InvoiceId { get; set; }
    public string? UserId { get; set; }
    public int? CustomerId { get; set; }
    public ApplicationUser? User { get; set; }
    public Customer? Customer { get; set; }
    public Supplier? Supplier { get; set; }

}
