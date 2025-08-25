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
    private readonly Dictionary<string, TokenValue> _emailVerificationTokens = new();
    private readonly Dictionary<string, TokenValue> _passwordResetTokens = new();
    private readonly Dictionary<string, TokenValue> _twoFactorTokens = new();
    private readonly Dictionary<string, TokenValue> _apiKeyTokens = new();
    private readonly Dictionary<string, TokenValue> _sessionTokens = new();

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
    /// تولید توکن تایید ایمیل
    /// </summary>
    public TokenValue GenerateEmailVerificationToken(string email, TimeSpan? expiresIn = null)
    {
        var token = CreateToken(TokenType.EmailVerification, email, null, expiresIn);
        _emailVerificationTokens[email] = token;
        return token;
    }

    /// <summary>
    /// تولید توکن بازیابی رمز عبور
    /// </summary>
    public TokenValue GeneratePasswordResetToken(string userId, TimeSpan? expiresIn = null)
    {
        var token = CreateToken(TokenType.PasswordReset, userId, null, expiresIn);
        _passwordResetTokens[userId] = token;
        return token;
    }

    /// <summary>
    /// تولید توکن احراز هویت دو عاملی
    /// </summary>
    public TokenValue GenerateTwoFactorToken(string userId, TimeSpan? expiresIn = null)
    {
        var token = CreateToken(TokenType.TwoFactor, userId, null, expiresIn);
        _twoFactorTokens[userId] = token;
        return token;
    }

    /// <summary>
    /// تولید کلید API
    /// </summary>
    public TokenValue GenerateApiKey(string userId, string applicationName, TimeSpan? expiresIn = null)
    {
        var token = CreateToken(TokenType.ApiKey, $"{userId}_{applicationName}", null, expiresIn);
        _apiKeyTokens[$"{userId}_{applicationName}"] = token;
        return token;
    }

    /// <summary>
    /// تولید توکن جلسه
    /// </summary>
    public TokenValue GenerateSessionToken(string userId, string sessionId, TimeSpan? expiresIn = null)
    {
        var token = CreateToken(TokenType.Session, $"{userId}_{sessionId}", null, expiresIn);
        _sessionTokens[$"{userId}_{sessionId}"] = token;
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
    /// اعتبارسنجی توکن تایید ایمیل
    /// </summary>
    public bool ValidateEmailVerificationToken(string token)
    {
        return _emailVerificationTokens.Values.Any(t => t.Value == token && !t.IsExpired);
    }

    /// <summary>
    /// اعتبارسنجی توکن بازیابی رمز عبور
    /// </summary>
    public bool ValidatePasswordResetToken(string token)
    {
        return _passwordResetTokens.Values.Any(t => t.Value == token && !t.IsExpired);
    }

    /// <summary>
    /// اعتبارسنجی توکن احراز هویت دو عاملی
    /// </summary>
    public bool ValidateTwoFactorToken(string token)
    {
        return _twoFactorTokens.Values.Any(t => t.Value == token && !t.IsExpired);
    }

    /// <summary>
    /// اعتبارسنجی کلید API
    /// </summary>
    public bool ValidateApiKey(string apiKey)
    {
        return _apiKeyTokens.Values.Any(t => t.Value == apiKey && !t.IsExpired);
    }

    /// <summary>
    /// ایجاد توکن سفارشی
    /// </summary>
    private TokenValue CreateToken(TokenType type, string identifier, string[] roles, TimeSpan? expiresIn)
    {
        var value = $"{type.ToString().ToUpper()}_{identifier}_{Guid.NewGuid():N}";
        var expiresAt = expiresIn.HasValue ?
            DateTime.UtcNow.Add(expiresIn.Value) :
            DateTime.UtcNow.AddHours(1);

        // استفاده از متد Create برای ایجاد توکن (طبق knowledge base)
        var token = TokenValue.Create(value, type, expiresAt);

        // افزودن اطلاعات اضافی اگر لازم باشد
        // در شبیه‌سازی معمولاً نیازی به این اطلاعات نیست
        return token;
    }

    /// <summary>
    /// ریست کردن وضعیت شبیه‌سازی
    /// </summary>
    public void Reset()
    {
        _tokens.Clear();
        _refreshTokens.Clear();
        _emailVerificationTokens.Clear();
        _passwordResetTokens.Clear();
        _twoFactorTokens.Clear();
        _apiKeyTokens.Clear();
        _sessionTokens.Clear();
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