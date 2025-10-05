namespace RowadSystem.Shard.Contract.Invoices;

public class SalesInvoiceItemRequest
{

    public string? ProductName { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public int UnitId { get; set; }
    public decimal Price { get; set; }
};
