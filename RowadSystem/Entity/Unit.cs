namespace RowadSystem.Entity;

public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<ProductUnit> productUnits { get; set; } = [];
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = [];

}
