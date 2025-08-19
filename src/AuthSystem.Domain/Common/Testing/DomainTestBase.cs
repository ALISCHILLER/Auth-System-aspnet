using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Common.Mocks;
using AuthSystem.Domain.Common.Policies;
using AuthSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AuthSystem.Domain.Common.Testing;

/// <summary>
/// کلاس پایه برای تست‌های دامنه
/// </summary>
public abstract class DomainTestBase : IDisposable
{
    /// <summary>
    /// سرویس‌های تست
    /// </summary>
    protected readonly IServiceProvider ServiceProvider;

    /// <summary>
    /// شبیه‌سازی زمان
    /// </summary>
    protected readonly MockClock Clock;

    /// <summary>
    /// شبیه‌سازی رمزنگاری
    /// </summary>
    protected readonly MockCryptoProvider CryptoProvider;

    /// <summary>
    /// شبیه‌سازی محدودکننده نرخ
    /// </summary>
    protected readonly MockRateLimiter RateLimiter;

    /// <summary>
    /// شبیه‌سازی تولیدکننده توکن
    /// </summary>
    protected readonly MockTokenGenerator TokenGenerator;

    /// <summary>
    /// سازنده با تنظیمات پیش‌فرض
    /// </summary>
    protected DomainTestBase()
    {
        var serviceCollection = new ServiceCollection();

        // افزودن شبیه‌سازی‌ها
        Clock = new MockClock();
        CryptoProvider = new MockCryptoProvider();
        RateLimiter = new MockRateLimiter();
        TokenGenerator = new MockTokenGenerator();

        serviceCollection.AddSingleton<ISystemClock>(Clock);
        serviceCollection.AddSingleton<ICryptoProvider>(CryptoProvider);
        serviceCollection.AddSingleton<IRateLimiter>(RateLimiter);
        serviceCollection.AddSingleton<ITokenGenerator>(TokenGenerator);

        // افزودن سرویس‌های دامنه
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    /// <summary>
    /// پیکربندی سرویس‌ها
    /// </summary>
    protected virtual void ConfigureServices(IServiceCollection services)
    {
        // پیکربندی پیش‌فرض - توسط کلاس‌های مشتق شده پیاده‌سازی می‌شود
    }

    /// <summary>
    /// ریست کردن شبیه‌سازی‌ها
    /// </summary>
    protected virtual void ResetMocks()
    {
        Clock.Reset();
        CryptoProvider.Reset();
        RateLimiter.Reset();
        TokenGenerator.Reset();
    }

    /// <summary>
    /// تنظیم زمان به مقدار مشخص
    /// </summary>
    protected void SetClock(DateTime utcNow)
    {
        Clock.SetUtcNow(utcNow);
    }

    /// <summary>
    /// جلو انداختن زمان
    /// </summary>
    protected void AdvanceClock(TimeSpan timeSpan)
    {
        Clock.Advance(timeSpan);
    }

    /// <summary>
    /// ایجاد یک موجودیت جدید
    /// </summary>
    protected T CreateEntity<T>(Action<T> setupAction = null) where T : BaseEntity<Guid>, new()
    {
        var entity = new T();
        setupAction?.Invoke(entity);
        return entity;
    }

    /// <summary>
    /// ایجاد یک موجودیت AggregateRoot جدید
    /// </summary>
    protected T CreateAggregate<T>(Action<T> setupAction = null) where T : AggregateRoot<Guid>, new()
    {
        var aggregate = new T();
        setupAction?.Invoke(aggregate);
        return aggregate;
    }

    /// <summary>
    /// شبیه‌سازی پردازش رویداد
    /// </summary>
    protected void ProcessDomainEvent(IDomainEvent @event)
    {
        // در تست‌ها می‌توان پردازش رویداد را شبیه‌سازی کرد
        // این فقط یک مثال است
        @event.MarkAsPublished();
    }

    /// <summary>
    /// شبیه‌سازی پردازش چندین رویداد
    /// </summary>
    protected void ProcessDomainEvents(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            ProcessDomainEvent(@event);
        }
    }

    /// <summary>
    /// ایجاد یک استثنا نمونه
    /// </summary>
    protected DomainException CreateSampleException()
    {
        return new DomainException("Test exception");
    }

    /// <summary>
    /// ایجاد یک استثنا نمونه برای قوانین
    /// </summary>
    protected BusinessRuleValidationException CreateSampleRuleException()
    {
        return new BusinessRuleValidationException("Rule violated", "TEST_RULE");
    }

    /// <summary>
    /// تست برای اطمینان از اینکه استثنا با پیام مشخص اتفاق افتاده است
    /// </summary>
    protected void AssertExceptionMessage<TException>(Action action, string expectedMessage)
        where TException : Exception
    {
        var exception = Assert.Throws<TException>(action);
        Assert.Contains(expectedMessage, exception.Message);
    }

    /// <summary>
    /// تست برای اطمینان از اینکه استثنا با پیام مشخص اتفاق افتاده است (ناهمزمان)
    /// </summary>
    protected async Task AssertExceptionMessageAsync<TException>(Func<Task> action, string expectedMessage)
        where TException : Exception
    {
        var exception = await Assert.ThrowsAsync<TException>(action);
        Assert.Contains(expectedMessage, exception.Message);
    }

    /// <summary>
    /// پاک کردن منابع
    /// </summary>
    public virtual void Dispose()
    {
        // پاک کردن منابع
    }
}