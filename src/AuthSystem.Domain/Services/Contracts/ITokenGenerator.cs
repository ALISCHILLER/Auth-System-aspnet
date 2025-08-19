using System;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Services.Contracts;

/// <summary>
/// اینترفیس برای تولید توکن‌های امنیتی
/// این اینترفیس قراردادهای لازم برای تولید انواع توکن‌های امنیتی را تعریف می‌کند
/// </summary>
public interface ITokenGenerator
{
    /// <summary>
    /// تولید توکن دسترسی
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="roles">نقش‌های کاربر</param>
    /// <param name="expiresIn">مدت زمان انقضا (اختیاری)</param>
    /// <returns>توکن دسترسی</returns>
    TokenValue GenerateAccessToken(string userId, string[] roles, TimeSpan? expiresIn = null);

    /// <summary>
    /// تولید توکن نوسازی
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <returns>توکن نوسازی</returns>
    string GenerateRefreshToken(string userId);

    /// <summary>
    /// تولید توکن تایید ایمیل
    /// </summary>
    /// <param name="email">آدرس ایمیل</param>
    /// <param name="expiresIn">مدت زمان انقضا (اختیاری)</param>
    /// <returns>توکن تایید ایمیل</returns>
    TokenValue GenerateEmailVerificationToken(string email, TimeSpan? expiresIn = null);

    /// <summary>
    /// تولید توکن بازیابی رمز عبور
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="expiresIn">مدت زمان انقضا (اختیاری)</param>
    /// <returns>توکن بازیابی رمز عبور</returns>
    TokenValue GeneratePasswordResetToken(string userId, TimeSpan? expiresIn = null);

    /// <summary>
    /// تولید توکن احراز هویت دو عاملی
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="expiresIn">مدت زمان انقضا (اختیاری)</param>
    /// <returns>توکن احراز هویت دو عاملی</returns>
    TokenValue GenerateTwoFactorToken(string userId, TimeSpan? expiresIn = null);

    /// <summary>
    /// تولید کلید API
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="applicationName">نام برنامه</param>
    /// <param name="expiresIn">مدت زمان انقضا (اختیاری)</param>
    /// <returns>کلید API</returns>
    TokenValue GenerateApiKey(string userId, string applicationName, TimeSpan? expiresIn = null);

    /// <summary>
    /// تولید توکن جلسه
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="sessionId">شناسه جلسه</param>
    /// <param name="expiresIn">مدت زمان انقضا (اختیاری)</param>
    /// <returns>توکن جلسه</returns>
    TokenValue GenerateSessionToken(string userId, string sessionId, TimeSpan? expiresIn = null);

    /// <summary>
    /// اعتبارسنجی توکن دسترسی
    /// </summary>
    /// <param name="token">توکن برای اعتبارسنجی</param>
    /// <returns>آیا توکن معتبر است</returns>
    bool ValidateAccessToken(string token);

    /// <summary>
    /// اعتبارسنجی توکن نوسازی
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="refreshToken">توکن نوسازی</param>
    /// <returns>آیا توکن معتبر است</returns>
    bool ValidateRefreshToken(string userId, string refreshToken);

    /// <summary>
    /// اعتبارسنجی توکن تایید ایمیل
    /// </summary>
    /// <param name="token">توکن برای اعتبارسنجی</param>
    /// <returns>آیا توکن معتبر است</returns>
    bool ValidateEmailVerificationToken(string token);

    /// <summary>
    /// اعتبارسنجی توکن بازیابی رمز عبور
    /// </summary>
    /// <param name="token">توکن برای اعتبارسنجی</param>
    /// <returns>آیا توکن معتبر است</returns>
    bool ValidatePasswordResetToken(string token);

    /// <summary>
    /// اعتبارسنجی توکن احراز هویت دو عاملی
    /// </summary>
    /// <param name="token">توکن برای اعتبارسنجی</param>
    /// <returns>آیا توکن معتبر است</returns>
    bool ValidateTwoFactorToken(string token);

    /// <summary>
    /// اعتبارسنجی کلید API
    /// </summary>
    /// <param name="apiKey">کلید API برای اعتبارسنجی</param>
    /// <returns>آیا کلید معتبر است</returns>
    bool ValidateApiKey(string apiKey);
}