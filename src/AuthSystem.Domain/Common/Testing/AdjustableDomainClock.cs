using System;
using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.Common.Testing;

/// <summary>
/// ساعتی قابل تنظیم برای سناریوهای تست دامنه.
/// </summary>
internal sealed class AdjustableDomainClock : IDomainClock
{
    private DateTime _utcNow;

    public AdjustableDomainClock(DateTime utcNow)
    {
        _utcNow = EnsureUtc(utcNow);
    }

    public DateTime UtcNow => _utcNow;

    public void SetFixedTime(DateTime utcNow)
    {
        _utcNow = EnsureUtc(utcNow);
    }

    public void Advance(TimeSpan offset)
    {
        _utcNow = _utcNow.Add(offset);
    }

    private static DateTime EnsureUtc(DateTime utcNow)
    {
        return utcNow.Kind == DateTimeKind.Utc ? utcNow : utcNow.ToUniversalTime();
    }
}