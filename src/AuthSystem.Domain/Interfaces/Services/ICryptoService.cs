using System;
using System.Collections.Generic;

namespace AuthSystem.Domain.Interfaces.Services;

/// <summary>
/// رابط اصلی برای سرویس‌های امنیتی و رمزنگاری
/// این رابط تمام عملیات امنیتی مورد نیاز در یک سیستم احراز هویت را تعریف می‌کند
/// </summary>
public interface ICryptoService
{
    /// <summary>
    /// تولید هش SHA256 از یک رشته ورودی
    /// </summary>
    /// <param name="input">رشته ورودی برای هش کردن</param>
    /// <returns>هش SHA256 به صورت Base64</returns>
    string GenerateSha256(string input);

    /// <summary>
    /// رمزنگاری یک رشته با استفاده از الگوریتم AES
    /// </summary>
    /// <param name="plainText">متن معمولی برای رمزنگاری</param>
    /// <returns>متن رمزنگاری شده</returns>
    string Encrypt(string plainText);

    /// <summary>
    /// رمزگشایی یک رشته رمزنگاری شده با AES
    /// </summary>
    /// <param name="encryptedText">متن رمزنگاری شده</param>
    /// <returns>متن رمزگشایی شده</returns>
    string Decrypt(string encryptedText);

    /// <summary>
    /// تولید یک توکن دسترسی (Access Token) با استفاده از JWT
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="email">ایمیل کاربر</param>
    /// <param name="roles">نقش‌های کاربر</param>
    /// <param name="additionalClaims">کلیم اضافی</param>
    /// <returns>توکن JWT</returns>
    string GenerateAccessToken(
        string userId,
        string email,
        List<string> roles,
        Dictionary<string, object>? additionalClaims = null);

    /// <summary>
    /// تولید یک توکن تازه‌سازی (Refresh Token)
    /// </summary>
    /// <returns>توکن تازه‌سازی</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// تولید همزمان توکن دسترسی و تازه‌سازی
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="email">ایمیل کاربر</param>
    /// <param name="roles">نقش‌های کاربر</param>
    /// <param name="additionalClaims">کلیم اضافی</param>
    /// <returns>توکن دسترسی و تازه‌سازی</returns>
    (string AccessToken, string RefreshToken) GenerateTokens(
        string userId,
        string email,
        List<string> roles,
        Dictionary<string, object>? additionalClaims = null);

    /// <summary>
    /// تولید یک توکن تأیید (Verification Token) برای تأیید ایمیل یا شماره تلفن
    /// </summary>
    /// <param name="purpose">هدف تولید توکن (مثلاً "EmailConfirmation", "PhoneVerification")</param>
    /// <param name="userId">شناسه کاربر</param>
    /// <returns>توکن تأیید</returns>
    string GenerateVerificationToken(string purpose, string userId);

    /// <summary>
    /// تولید یک کلید یکتا برای کاربر (مثلاً برای 2FA)
    /// </summary>
    /// <returns>کلید یکتا</returns>
    string GenerateUserSecretKey();

    /// <summary>
    /// بررسی صحت یک کد یک‌بار مصرف (OTP)
    /// </summary>
    /// <param name="secretKey">کلید محرمانه کاربر</param>
    /// <param name="code">کد ورودی کاربر</param>
    /// <returns>در صورت صحت true باز می‌گرداند</returns>
    bool VerifyOtpCode(string secretKey, string code);

    /// <summary>
    /// تولید یک کد یک‌بار مصرف (OTP) برای احراز هویت دو مرحله‌ای
    /// </summary>
    /// <param name="secretKey">کلید محرمانه کاربر</param>
    /// <returns>کد یک‌بار مصرف</returns>
    string GenerateOtpCode(string secretKey);

    /// <summary>
    /// تولید یک توکن موقت برای بازیابی رمز عبور
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <returns>توکن بازیابی رمز عبور</returns>
    string GeneratePasswordResetToken(string userId);

    /// <summary>
    /// تولید یک توکن امن برای ارتباطات داخلی
    /// </summary>
    /// <returns>توکن امن</returns>
    string GenerateInternalToken();
}