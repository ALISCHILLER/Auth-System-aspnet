using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// استثنا برای زمانی که خطای هم‌زمانی (Concurrency) رخ داده باشد.
/// معمولاً به کد وضعیت HTTP 409 (Conflict) نگاشت می‌شود.
/// مثال: شکست به دلیل optimistic concurrency.
/// </summary>
public sealed class ConcurrencyException : ApplicationException
{
    /// <summary>
    /// ساخت یک استثنا با پیام اختیاری.
    /// </summary>
    public ConcurrencyException(string message = "خطا در هم‌زمانی داده‌ها — داده توسط کاربر دیگری تغییر کرده است.")
        : base("concurrency_conflict", message, new[] { Error.Conflict(message) })
    {
    }
}