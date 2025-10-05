namespace RowadSystem.API.Entity;

public class CashierHandover
{
    public int Id { get; set; }
    public string CashierId { get; set; } = null!;
    public decimal AmountInDrawer { get; set; }
    public decimal TotalSalesAmount { get; set; }
    public decimal TotalReturnAmount { get; set; }
    public decimal TotalPurchaseAmount { get; set; }
    public decimal TotalPurchaseReturnAmount { get; set; }
    public decimal Difference { get; set; }
    public DateTime HandoverDate { get; set; }
    public DateTime? HandoverStartDate { get; set; }
    public DateTime? HandoverEndDate { get; set; }
    public string? Notes { get; set; }
}
