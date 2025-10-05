
namespace RowadSystem.Shard.Contract.Invoices;
public class AccountStatementDto
{
    public int InvoiceId { get; set; } 
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal Amount { get; set; }
    public decimal Payments { get; set; }
    public decimal BalanceDue { get; set; }
}
