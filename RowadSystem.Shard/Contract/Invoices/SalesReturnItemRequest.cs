namespace RowadSystem.Shard.Contract.Invoices;

//public record SalesReturnItemRequest
//(
//    int ProductId,
//    int Quantity,
//    int UnitId

// );

public class SalesReturnItemRequest
{
    public string? ProductName { get; set; }
    public string? UnitName { get; set; }

    public int ProductId { get; set; }
    public int Quantity { get; set; } 
    public int UnitId { get; set; }
}
