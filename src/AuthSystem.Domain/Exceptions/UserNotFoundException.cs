// File: AuthSystem.Domain/Exceptions/UserNotFoundException.cs
using AuthSystem.Domain.Common.Exceptions;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای یافتن نشدن کاربر
/// - هنگام جستجوی کاربر بر اساس شناسه یا ایمیل رخ می‌دهد
/// - شامل جزئیات جستجو برای ارائه پیام مناسب به کاربر
/// </summary>
public class UserNotFoundException : DomainException
{
    /// <summary>
    /// شناسه کاربر (در صورت جستجو بر اساس شناسه)
    /// </summary>
    public Guid? UserId { get; }

    /// <summary>
    /// آدرس ایمیل (در صورت جستجو بر اساس ایمیل)
    /// </summary>
    public string? Email { get; }

    /// <summary>
    /// نام کاربری (در صورت جستجو بر اساس نام کاربری)
    /// </summary>
    public string? Username { get; }

    /// <summary>
    /// سازنده پرایوت
    /// </summary>
    private UserNotFoundException(string message, string errorCode, Guid? userId = null, string? email = null, string? username = null)
        : base(message, errorCode)
    {
        UserId = userId;
        Email = email;
        Username = username;

        if (userId.HasValue)
            Data.Add("UserId", userId.Value);
        if (!string.IsNullOrEmpty(email))
            Data.Add("Email", email);
        if (!string.IsNullOrEmpty(username))
            Data.Add("Username", username);
    }

    /// <summary>
    /// سازنده استاتیک برای جستجو بر اساس شناسه
    /// </summary>
    public static UserNotFoundException ById(Guid userId)
        => new UserNotFoundException(
            $"کاربر با شناسه '{userId}' یافت نشد",
            "USER_NOT_FOUND_BY_ID",
            userId: userId);

    /// <summary>
    /// سازنده استاتیک برای جستجو بر اساس ایمیل
    /// </summary>
    public static UserNotFoundException ByEmail(string email)
        => new UserNotFoundException(
            $"کاربری با آدرس ایمیل '{email}' یافت نشد",
            "USER_NOT_FOUND_BY_EMAIL",
            email: email);

    /// <summary>
    /// سازنده استاتیک برای جستجو بر اساس نام کاربری
    /// </summary>
    public static UserNotFoundException ByUsername(string username, bool caseSensitive = false)
        => new UserNotFoundException(
            $"کاربری با نام کاربری '{username}' {(caseSensitive ? "یافت نشد" : "یافت نشد (حساس به بزرگی و کوچکی حروف)")}",
            "USER_NOT_FOUND_BY_USERNAME",
            username: username);

    /// <summary>
    /// سازنده استاتیک برای استفاده عمومی
    /// </summary>
    public static UserNotFoundException General()
        => new UserNotFoundException(
            "کاربر مورد نظر یافت نشد",
            "USER_NOT_FOUND");
}