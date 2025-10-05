using System.Text.Json.Serialization;

namespace RowadSystem.Entity;

public class ProductImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string PublicId { get; set; } = null!;
    public int ProductId { get; set; }
    [JsonIgnore]
    public Product? Product { get; set; }
}
