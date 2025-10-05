namespace RowadSystem.Shard.Contract.Products;

//public record UnitWithConversionDto
//(
//     int UnitId,
//     int QuantityInBaseUnit,
//     bool IsBaseUnit,
//     decimal SellingPrice
//);

public class UnitWithConversionDto
{
    public int UnitId { get; set; }
    public int QuantityInBaseUnit { get; set; }
    public bool IsBaseUnit { get; set; }
    public decimal SellingPrice { get; set; }
    //public UnitWithConversionDto(int unitId, int quantityInBaseUnit, bool isBaseUnit, decimal sellingPrice)
    //{
    //    UnitId = unitId;
    //    QuantityInBaseUnit = quantityInBaseUnit;
    //    IsBaseUnit = isBaseUnit;
    //    SellingPrice = sellingPrice;
    //}
}