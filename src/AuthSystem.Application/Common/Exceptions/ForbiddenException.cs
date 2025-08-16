using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// استثنا برای زمانی که کاربر احراز هویت شده اما دسترسی لازم را ندارد.
/// معمولاً به کد وضعیت HTTP 403 (Forbidden) نگاشت می‌شود.
/// </summary>
public sealed class ForbiddenException : ApplicationException
{
    /// <summary>
    /// ساخت یک استثنا با پیام اختیاری.
    /// </summary>
    public ForbiddenException(string message = "دسترسی غیرمجاز — کاربر دسترسی لازم را ندارد.")
        : base("forbidden", message, new[] { Error.Forbidden(message) })
    {
    }
}