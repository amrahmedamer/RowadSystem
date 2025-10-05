namespace RowadSystem.Entity;
public class OTP
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser? User { get; set; }
    public string code { get; set; } = null!;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(5);
    public bool IsUsed { get; set; } = false;


}
