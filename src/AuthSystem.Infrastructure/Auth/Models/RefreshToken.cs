namespace AuthSystem.Infrastructure.Auth.Models;

public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Hash { get; set; } = default!;
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? ReplacedByTokenHash { get; set; }
    public string? Ip { get; set; }
    public string? UserAgent { get; set; }

    public bool IsRevoked => RevokedAtUtc is not null;
    public bool IsExpired => ExpiresAtUtc <= DateTime.UtcNow;
}