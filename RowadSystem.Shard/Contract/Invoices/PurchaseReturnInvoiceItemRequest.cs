namespace RowadSystem.Shard.Contract.Invoices;

//public record PurchaseReturnInvoiceItemRequest
//(
//    int ProductId,
//    int Quantity,
//    int UnitId
//);
public class PurchaseReturnInvoiceItemRequest
{
    public string? ProductName { get; set; }
    public string? UnitName { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int UnitId { get; set; }
};
