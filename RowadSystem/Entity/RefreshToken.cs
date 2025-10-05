namespace RowadSystem.Entity;
[Owned]
public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public bool IsActive => DateTime.UtcNow < Expiration;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
    public bool IsRevoked => RevokedAt.HasValue && RevokedAt.Value < DateTime.UtcNow;


}
