using System;
using AuthSystem.Domain.Enums; // اضافه کردن این خط برای دسترسی به HashAlgorithm
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Services.Contracts;

/// <summary>
/// اینترفیس برای هش کردن رمز عبور
/// این اینترفیس قراردادهای لازم برای ایمن‌سازی رمز عبور را تعریف می‌کند
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// هش کردن رمز عبور با استفاده از الگوریتم پیش‌فرض
    /// </summary>
    /// <param name="password">رمز عبور خام</param>
    /// <returns>هش رمز عبور به عنوان یک Value Object</returns>
    PasswordHash HashPassword(string password);

    /// <summary>
    /// هش کردن رمز عبور با استفاده از الگوریتم مشخص
    /// </summary>
    /// <param name="password">رمز عبور خام</param>
    /// <param name="algorithm">الگوریتم هش</param>
    /// <returns>هش رمز عبور به عنوان یک Value Object</returns>
    PasswordHash HashPassword(string password, HashAlgorithm algorithm); // تغییر از PasswordHash.HashAlgorithm به HashAlgorithm

    /// <summary>
    /// تأیید رمز عبور با هش موجود
    /// </summary>
    /// <param name="password">رمز عبور خام</param>
    /// <param name="hashedPassword">هش رمز عبور</param>
    /// <returns>آیا رمز عبور معتبر است</returns>
    bool VerifyPassword(string password, PasswordHash hashedPassword);

    /// <summary>
    /// تأیید رمز عبور با هش موجود و بررسی نیاز به ری‌هش
    /// </summary>
    /// <param name="password">رمز عبور خام</param>
    /// <param name="hashedPassword">هش رمز عبور</param>
    /// <param name="needsRehash">آیا نیاز به ری‌هش دارد</param>
    /// <returns>آیا رمز عبور معتبر است</returns>
    bool VerifyPassword(string password, PasswordHash hashedPassword, out bool needsRehash);

    /// <summary>
    /// تولید رمز عبور موقت ایمن
    /// </summary>
    /// <param name="length">طول رمز عبور (پیش‌فرض 12 کاراکتر)</param>
    /// <returns>رمز عبور موقت ایمن</returns>
    string GenerateTemporaryPassword(int length = 12);

    /// <summary>
    /// آیا هش رمز عبور نیاز به به‌روزرسانی دارد
    /// </summary>
    /// <param name="hashedPassword">هش رمز عبور</param>
    /// <returns>نیاز به ری‌هش</returns>
    bool NeedsRehash(PasswordHash hashedPassword);
}