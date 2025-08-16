using System;
using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// استثنا برای زمانی که یک منبع (مثلاً کاربر، نقش) یافت نشد.
/// معمولاً به کد وضعیت HTTP 404 (Not Found) نگاشت می‌شود.
/// </summary>
public sealed class NotFoundException : ApplicationException
{
    /// <summary>
    /// نام منبعی که یافت نشد (مثلاً "User").
    /// </summary>
    public string Resource { get; }

    /// <summary>
    /// شناسهٔ منبع (اختیاری).
    /// </summary>
    public object? Key { get; }

    /// <summary>
    /// ساخت یک استثنا با نام منبع و شناسهٔ آن.
    /// </summary>
    public NotFoundException(string resource, object? key = null)
        : base(
            "not_found",
            key is null ? $"{resource} یافت نشد." : $"{resource} با شناسه '{key}' یافت نشد.",
            new[] { Error.NotFound(key is null ? resource : $"{resource} ({key})") })
    {
        Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        Key = key;
    }
}