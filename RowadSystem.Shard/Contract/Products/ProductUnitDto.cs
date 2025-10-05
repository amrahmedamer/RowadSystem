namespace RowadSystem.Shard.Contract.Products;

public class  ProductUnitDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfUnits { get; set; }
    public decimal Price { get; set; }
    public bool IsBaseUnit { get; set; }
};
//public record class ProductUnitDto
//(
//    int Id,
//    string Name,
//    int NumberOfUnits,
//    decimal SellingPrice
//);