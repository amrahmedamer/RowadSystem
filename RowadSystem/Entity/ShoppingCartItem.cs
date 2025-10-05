namespace RowadSystem.Entity;

public class ShoppingCartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public int UnitId { get; set; }
    public Unit? Unit { get; set; }
    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
