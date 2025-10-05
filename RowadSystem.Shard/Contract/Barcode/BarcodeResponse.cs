

namespace RowadSystem.Shard.Contract.Barcode;
public class BarcodeResponse
{
    public byte[]? BarcodeImage { get; set; }
    public string ProductCode { get; set; } = string.Empty;
     
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }



}
