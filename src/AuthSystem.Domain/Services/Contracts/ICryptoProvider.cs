using System;
using System.Collections.Generic;
using System.Text;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Services.Contracts;

/// <summary>
/// اینترفیس برای ارائه‌دهنده سرویس‌های رمزنگاری
/// این اینترفیس قراردادهای لازم برای انجام عملیات رمزنگاری و هش کردن را تعریف می‌کند
/// </summary>
public interface ICryptoProvider
{
    /// <summary>
    /// رمزنگاری داده با استفاده از کلید مشخص
    /// </summary>
    /// <param name="plainText">متن خام برای رمزنگاری</param>
    /// <param name="key">کلید رمزنگاری</param>
    /// <returns>داده رمزنگاری شده به صورت رشته</returns>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن plainText یا key</exception>
    /// <exception cref="CryptographicException">در صورت بروز خطا در فرآیند رمزنگاری</exception>
    string Encrypt(string plainText, string key);

    /// <summary>
    /// رمزگشایی داده رمزنگاری شده با استفاده از کلید مشخص
    /// </summary>
    /// <param name="cipherText">داده رمزنگاری شده برای رمزگشایی</param>
    /// <param name="key">کلید رمزگشایی</param>
    /// <returns>متن خام رمزگشایی شده</returns>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن cipherText یا key</exception>
    /// <exception cref="CryptographicException">در صورت بروز خطا در فرآیند رمزگشایی</exception>
    string Decrypt(string cipherText, string key);

    /// <summary>
    /// ایجاد کلید رمزنگاری تصادفی با طول مشخص
    /// </summary>
    /// <param name="length">طول کلید به بایت (پیش‌فرض 32 بایت)</param>
    /// <returns>کلید تصادفی به صورت Base64</returns>
    string GenerateRandomKey(int length = 32);

    /// <summary>
    /// ایجاد IV (Initialization Vector) تصادفی با طول مشخص
    /// </summary>
    /// <param name="length">طول IV به بایت (پیش‌فرض 16 بایت)</param>
    /// <returns>IV تصادفی به صورت Base64</returns>
    string GenerateRandomIv(int length = 16);

    /// <summary>
    /// هش کردن داده با استفاده از الگوریتم مشخص و نمک
    /// </summary>
    /// <param name="input">داده ورودی برای هش کردن</param>
    /// <param name="salt">نمک برای هش کردن (اختیاری)</param>
    /// <param name="algorithm">الگوریتم هش (پیش‌فرض SHA256)</param>
    /// <returns>هش شده داده به صورت Base64</returns>
    /// <exception cref="ArgumentNullException">در صورت خالی بودن input</exception>
    string Hash(string input, string salt = null, HashAlgorithm algorithm = HashAlgorithm.SHA256);

    /// <summary>
    /// تأیید هش داده با استفاده از هش موجود و نمک
    /// </summary>
    /// <param name="input">داده ورودی برای هش کردن</param>
    /// <param name="hash">هش موجود برای مقایسه</param>
    /// <param name="salt">نمک استفاده شده در هش اصلی (اختیاری)</param>
    /// <param name="algorithm">الگوریتم هش (پیش‌فرض SHA256)</param>
    /// <returns>درست بودن هش</returns>
    bool VerifyHash(string input, string hash, string salt = null, HashAlgorithm algorithm = HashAlgorithm.SHA256);

    /// <summary>
    /// ایجاد توکن امن تصادفی با طول مشخص
    /// </summary>
    /// <param name="length">طول توکن (پیش‌فرض 64 کاراکتر)</param>
    /// <returns>توکن امن تصادفی</returns>
    string GenerateSecureToken(int length = 64);

    /// <summary>
    /// رمزنگاری داده با استفاده از کلید عمومی
    /// </summary>
    /// <param name="plainText">متن خام برای رمزنگاری</param>
    /// <param name="publicKey">کلید عمومی</param>
    /// <returns>داده رمزنگاری شده به صورت Base64</returns>
    string EncryptWithPublicKey(string plainText, string publicKey);

    /// <summary>
    /// رمزگشایی داده با استفاده از کلید خصوصی
    /// </summary>
    /// <param name="cipherText">داده رمزنگاری شده برای رمزگشایی</param>
    /// <param name="privateKey">کلید خصوصی</param>
    /// <returns>متن خام رمزگشایی شده</returns>
    string DecryptWithPrivateKey(string cipherText, string privateKey);

    /// <summary>
    /// ایجاد امضای دیجیتال برای داده
    /// </summary>
    /// <param name="data">داده برای امضا</param>
    /// <param name="privateKey">کلید خصوصی</param>
    /// <returns>امضای دیجیتال به صورت Base64</returns>
    string SignData(string data, string privateKey);

    /// <summary>
    /// تأیید امضای دیجیتال برای داده
    /// </summary>
    /// <param name="data">داده اصلی</param>
    /// <param name="signature">امضای دیجیتال</param>
    /// <param name="publicKey">کلید عمومی</param>
    /// <returns>اعتبار امضا</returns>
    bool VerifySignature(string data, string signature, string publicKey);
}