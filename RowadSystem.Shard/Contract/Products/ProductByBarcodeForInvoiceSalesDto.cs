
using RowadSystem.Shard.Contract.Units;

namespace RowadSystem.Shard.Contract.Products;
public class ProductByBarcodeForInvoiceSalesDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public List<ProductUnitDto> Units { get; set; } = new();  
    public string Status { get; set; } = string.Empty; 
    public int AvailableStock { get; set; }                // Available stock quantity
    public decimal Discount { get; set; } = 0;            
    public decimal FinalPrice { get; set; }     // Price after discount (if any).          
}
