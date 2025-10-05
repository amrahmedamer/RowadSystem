namespace RowadSystem.Shard.Contract.Auth;

public record AuthResponse(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    string Token,
    int Expiration,
    string RefreshToken,
    DateTime RefreshTokenExpiration
 );
