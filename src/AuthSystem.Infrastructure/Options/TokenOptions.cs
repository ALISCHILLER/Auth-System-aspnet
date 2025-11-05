namespace AuthSystem.Infrastructure.Options;

public sealed class TokenOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = "AuthSystem";
    public string Audience { get; set; } = "AuthSystem.Clients";
    public string SigningKey { get; set; } = "CHANGE-ME-TO-A-VERY-LONG-SECRET-VALUE";
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 14;
    public bool EnableRefreshTokenReuseDetection { get; set; } = true;
}