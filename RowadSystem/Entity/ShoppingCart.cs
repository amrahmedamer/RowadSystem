using System.ComponentModel.DataAnnotations;

namespace RowadSystem.Entity;

public class ShoppingCart
{
    [Key]
    public int Id { get; set; }
    public DateTime ShoppingCartDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }
    public ShoppingCartStatus Status { get; set; } = ShoppingCartStatus.Pending;
    public string? GuestId { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public ICollection<ShoppingCartItem> shoppingCartItems { get; set; } = [];
}
