namespace RowadSystem.Entity;

public abstract class Invoice : AuditableEntity
{
    public int Id { get; set; }
    public DateTime InvoiceDate { get; set; } = DateTime.Now;
    public string InvoiceNumber { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public decimal TotalAmount { get; set; } 
    //public bool IsPaid { get; set; } = false;
    public PaymentStatus PaymentStatus { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

}
