using System;
using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.Common.Testing;

/// <summary>
/// پایه‌ای سبک برای تست‌های دامنه که فقط به زیرساخت‌های درون دامنه متکی است.
/// این کلاس از <see cref="DomainClock"/> برای کنترل زمان در سناریوهای تست بهره می‌برد.
/// </summary>
public abstract class DomainTestBase : IDisposable
{

    private AdjustableDomainClock? _testClock;

    /// <summary>
    /// نمونه ساعت دامنه که به تست‌ها اجازه می‌دهد زمان را مشاهده کنند.
    /// </summary>
    protected IDomainClock Clock => DomainClock.Instance;

    /// <summary>
    /// زمان ثابت برای سناریوهای تست تعیین می‌کند.
    /// </summary>
    protected void FreezeTime(DateTime utcNow)
    {
        _testClock = new AdjustableDomainClock(utcNow);
        DomainClock.Set(_testClock);
    }

    /// <summary>
    /// تغییر زمان نسبت به مقدار فعلی (در صورت فریز بودن) را فراهم می‌کند.
    /// </summary>
    protected void AdvanceTime(TimeSpan offset)
    {
        if (_testClock is null)
        {
            FreezeTime(DomainClock.Instance.UtcNow.Add(offset));
            return;
        }

        _testClock.Advance(offset);
    }

    /// <summary>
    /// ریست کردن ساعت دامنه پس از اتمام تست.
    /// </summary>
    protected void ResetTime()
    {
        _testClock = null;
        DomainClock.Reset();
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        ResetTime();
        GC.SuppressFinalize(this);
    }
}