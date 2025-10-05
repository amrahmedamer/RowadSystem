namespace RowadSystem.Shard.Contract.Invoices;

public class PurchaseInvoiceItemRequest
{
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public int Quantity { get; set; } 
    public decimal Price { get; set; }
    public string? ProductName { get; set; }
    public string? UnitName { get; set; }
};

