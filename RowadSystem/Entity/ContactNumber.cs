namespace RowadSystem.Entity;

public class ContactNumber
{
    public int Id { get; set; }
    public string Number { get; set; } = null!;
    public bool IsPrimary { get; set; } = false;

    public string? UserId { get; set; }
    public int? CustomerId { get; set; }
    public int? SupplierId { get; set; }

    public ApplicationUser? User { get; set; }
    public Customer? Customer { get; set; }
    public Supplier? Supplier { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}
