namespace RowadSystem.Entity;

public class ProductUnit
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int UnitId { get; set; }
    public Unit? Unit { get; set; }
    public int QuantityInBaseUnit { get; set; }
    public decimal SellingPrice { get; set; }
    public bool IsBaseUnit { get; set; } = false;

}
