namespace RowadSystem.Entity;

public class Coupon
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public bool IsPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    //  public bool ExpirationDate => EndDate < DateTime.UtcNow;
    public ICollection<SalesInvoice> SalesInvoices { get; set; } = [];

}
