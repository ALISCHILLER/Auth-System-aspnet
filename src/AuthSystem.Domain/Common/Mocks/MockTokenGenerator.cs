using System;
using System.Collections.Generic;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Services.Contracts;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Common.Mocks;

/// <summary>
/// شبیه‌سازی تولیدکننده توکن برای تست‌ها
/// </summary>
public class MockTokenGenerator : ITokenGenerator
{
    private readonly Dictionary<string, TokenValue> _tokens = new();
    private readonly Dictionary<string, string> _refreshTokens = new();

    /// <summary>
    /// تولید توکن دسترسی
    /// </summary>
    public TokenValue GenerateAccessToken(string userId, string[] roles, TimeSpan? expiresIn = null)
    {
        var token = CreateToken(TokenType.Access, userId, roles, expiresIn);
        _tokens[userId] = token;
        return token;
    }

    /// <summary>
    /// تولید توکن نوسازی
    /// </summary>
    public string GenerateRefreshToken(string userId)
    {
        var token = $"REFRESH_{userId}_{Guid.NewGuid():N}";
        _refreshTokens[userId] = token;
        return token;
    }

    /// <summary>
    /// اعتبارسنجی توکن دسترسی
    /// </summary>
    public bool ValidateAccessToken(string token)
    {
        return _tokens.Values.Any(t => t.Value == token && !t.IsExpired);
    }

    /// <summary>
    /// اعتبارسنجی توکن نوسازی
    /// </summary>
    public bool ValidateRefreshToken(string userId, string refreshToken)
    {
        return _refreshTokens.TryGetValue(userId, out var storedToken) &&
               storedToken == refreshToken;
    }

    /// <summary>
    /// ایجاد توکن سفارشی
    /// </summary>
    private TokenValue CreateToken(TokenType type, string userId, string[] roles, TimeSpan? expiresIn)
    {
        var value = $"{type.ToString().ToUpper()}_{userId}_{Guid.NewGuid():N}";
        var expiresAt = expiresIn.HasValue ?
            DateTime.UtcNow.Add(expiresIn.Value) :
            DateTime.UtcNow.AddHours(1);

        return new TokenValue(value, type, expiresAt)
        {
            // افزودن اطلاعات اضافی
            AddMetadata("userId", userId),
            AddMetadata("roles", roles)
        };
    }

    /// <summary>
    /// ریست کردن وضعیت شبیه‌سازی
    /// </summary>
    public void Reset()
    {
        _tokens.Clear();
        _refreshTokens.Clear();
    }

    /// <summary>
    /// شبیه‌سازی انقضای توکن
    /// </summary>
    public void ExpireToken(string userId)
    {
        if (_tokens.TryGetValue(userId, out var token))
        {
            _tokens[userId] = token.Expire();
        }
    }
}