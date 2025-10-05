using RowadSystem.API.Entity;

namespace RowadSystem.Entity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public ICollection<OTP> Otp { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<Invoice> Invoices { get; set; } = [];
    public ICollection<ShoppingCart> ShoppingCarts { get; set; } = [];
    public ICollection<ContactNumber> ContactNumbers { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;

    public int? AddressId { get; set; }
    public Address Address { get; set; } = null!;

}
