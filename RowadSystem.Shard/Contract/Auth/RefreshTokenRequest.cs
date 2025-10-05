namespace RowadSystem.Shard.Contract.Auth;

public record RefreshTokenRequest
(
    string token,
    string refreshToken
);
