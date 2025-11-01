using System;

namespace AuthSystem.Infrastructure.Tokens;

public sealed class TokenOptions
{
    public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(15);
    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(7);
}