namespace RowadSystem.Shard.Abstractions;

public record Error(string code, string description, int statusCode)
{
    public static Error None = new Error("", "", 0);
}
