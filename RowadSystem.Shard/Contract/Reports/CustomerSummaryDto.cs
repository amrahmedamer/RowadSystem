

namespace RowadSystem.Shard.Contract.Reports;
public class CustomerSummaryDto
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime RegisterDate { get; set; }
    public string Status { get; set; } = "Active"; // Active / InActive / Pending
}
