using System.Text.Json.Serialization;

namespace RowadSystem.Entity;

public class Discount
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPercentage { get; set; }
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = [];
}
