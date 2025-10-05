

namespace RowadSystem.Shard.Abstractions;
public class ProblemDetails
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, object?> Extensions { get; set; } = new Dictionary<string, object?>();
}
public class ErrorDetail
{
    public string Code { get; set; }
    public string Description { get; set; }
    public int StatusCode { get; set; }
}