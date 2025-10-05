namespace RowadSystem.Shard.Contract.Auth;

//public record ConfirmEmailRequest(
//    string Email,
//    string Code
//);

public record ConfirmEmailRequest
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

}