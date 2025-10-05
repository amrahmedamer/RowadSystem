namespace RowadSystem.Shard.Contract.Orders;

public class OrderItemResponse
{
    public int Id { get; set; }
    public string ProductName { get; set; }= string.Empty;
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int UnitId { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
};
