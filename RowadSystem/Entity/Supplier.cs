using RowadSystem.API.Entity;

namespace RowadSystem.Entity;

public class Supplier : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<PurchaseInvoice>? PurchaseInvoices { get; set; }
    public ICollection<ContactNumber>? ContactNumbers { get; set; } = [];
    public int? AddressId { get; set; }
    public Address Address { get; set; } = null!;


}
