using System;

namespace AuthSystem.Application.DTOs.Responses;

/// <summary>
/// مدل پاسخ شامل توکن‌های دسترسی و تازه‌سازی
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// توکن دسترسی جدید
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// توکن تازه‌سازی جدید
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// تاریخ و زمان انقضای توکن دسترسی
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// تاریخ و زمان انقضای توکن تازه‌سازی
    /// </summary>
    public DateTime? RefreshTokenExpiresAt { get; set; }
}