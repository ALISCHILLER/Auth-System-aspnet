using System;
using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// استثنا برای زمانی که تداخل (Conflict) رخ داده باشد.
/// معمولاً به کد وضعیت HTTP 409 (Conflict) نگاشت می‌شود.
/// مثال: نقض محدودیت Unique یا قفل هم‌زمان.
/// </summary>
public sealed class ConflictException : ApplicationException
{
    /// <summary>
    /// نام منبعی که تداخل در آن رخ داده است.
    /// </summary>
    public string Resource { get; }

    /// <summary>
    /// ساخت یک استثنا با نام منبع.
    /// </summary>
    public ConflictException(string resource, string? message = null)
        : base(
            "conflict",
            message ?? $"تداخل در {resource} رخ داده است.",
            new[] { Error.Conflict(message ?? $"تداخل در {resource} رخ داده است.") })
    {
        Resource = resource ?? throw new ArgumentNullException(nameof(resource));
    }
}