namespace RowadSystem.Entity;

public class InvoiceDetail
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public Product? Product { get; set; }
    public int UnitId { get; set; }
    public Unit? Unit { get; set; }
}

