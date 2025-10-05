
using RowadSystem.Shard.consts.Enums;
using RowadSystem.Shard.Contract.Products;

namespace RowadSystem.Shard.Contract.Invoices;
public class InvoiceItemResponse
{
    public string Barcode { get; set; } = null!;
    public int ProductId { get; set; } 
    public string ProductName { get; set; } = null!;
    public string UnitName { get; set; } = null!;
    public int UnitId { get; set; }
    public decimal UnitPrice { get; set; } 
    public int Quantity { get; set; } 
    public decimal TotalPrice { get; set; }

    public ProductUnitDto productUnits { get; set; } =new();

    public string Status =>
        Quantity > 0 ? ProductStatus.Active.ToString()
      : Quantity == 0 ? ProductStatus.OutOfStock.ToString()
      : ProductStatus.Inactive.ToString();


}
