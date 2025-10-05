namespace RowadSystem.Shard.Contract.Helpers;
public class RequestFilters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchValue { get; set; }

}
