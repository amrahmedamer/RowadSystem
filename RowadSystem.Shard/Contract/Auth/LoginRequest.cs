namespace RowadSystem.Shard.Contract.Auth;

//public record LoginRequest(

//    string Email,
//    string Password
//);

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

}
