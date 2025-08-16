using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// استثنا برای زمانی که کاربر احراز هویت نشده است.
/// معمولاً به کد وضعیت HTTP 401 (Unauthorized) نگاشت می‌شود.
/// </summary>
public sealed class UnauthorizedException : ApplicationException
{
    /// <summary>
    /// ساخت یک استثنا با پیام اختیاری.
    /// </summary>
    public UnauthorizedException(string message = "دسترسی غیرمجاز — کاربر احراز هویت نشده است.")
        : base("unauthorized", message, new[] { Error.Unauthorized(message) })
    {
    }
}