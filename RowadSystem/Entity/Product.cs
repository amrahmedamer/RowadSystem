
namespace RowadSystem.Entity;

public class Product : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal PurchasePrice { get; set; }
    public int CategoryId { get; set; }
    //public int SupplierId { get; set; }
    public int? BrandId { get; set; }
    public int? DiscountId { get; set; }
    public string? Barcode { get; set; }
    public Category? Category { get; set; }
    //public Supplier? Supplier { get; set; }
    public Brand? Brand { get; set; }
    public Discount? Discount { get; set; }
    public bool IsDiscontinued { get; set; } = false;  
    public bool IsInactive { get; set; } = false;
    public ProductStatus Status
    {
        get
        {
            if (IsDeleted || IsDiscontinued || IsInactive)
                return ProductStatus.Inactive;

            if (Inventory?.Quantity > 0)
                return ProductStatus.Active;

            return ProductStatus.OutOfStock;
        }
    }
    public Inventory? Inventory { get; set; }
    public ICollection<ProductImage>? Images { get; set; }
    public ICollection<ProductUnit> productUnits { get; set; } = [];
    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = [];




}
