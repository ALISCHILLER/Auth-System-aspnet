using System;
using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.Common.Testing;

/// <summary>
/// پایه‌ای سبک برای تست‌های دامنه که فقط به زیرساخت‌های درون دامنه متکی است.
/// این کلاس از <see cref="DomainClock"/> برای کنترل زمان در سناریوهای تست بهره می‌برد.
/// </summary>
public abstract class DomainTestBase : IDisposable
{
    /// <summary>
    /// نمونه ساعت دامنه که به تست‌ها اجازه می‌دهد زمان را کنترل کنند.
    /// </summary>
    protected DomainClock Clock => DomainClock.Instance;

    /// <summary>
    /// زمان ثابت برای سناریوهای تست تعیین می‌کند.
    /// </summary>
    protected void FreezeTime(DateTime utcNow) => Clock.SetFixedTime(utcNow);

    /// <summary>
    /// تغییر زمان نسبت به مقدار فعلی (در صورت فریز بودن) را فراهم می‌کند.
    /// </summary>
    protected void AdvanceTime(TimeSpan offset) => Clock.Advance(offset);

    /// <summary>
    /// ریست کردن ساعت دامنه پس از اتمام تست.
    /// </summary>
    protected void ResetTime() => Clock.Reset();

    /// <inheritdoc />
    public virtual void Dispose()
    {
        Clock.Reset();
    }
}