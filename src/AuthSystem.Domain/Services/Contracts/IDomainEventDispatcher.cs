using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Services.Contracts;

/// <summary>
/// اینترفیس برای ارسال رویدادهای دامنه
/// این اینترفیس مسئول ارسال رویدادهای دامنه به مصرف‌کننده‌های آن است
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// ارسال یک رویداد دامنه
    /// </summary>
    /// <typeparam name="TEvent">نوع رویداد</typeparam>
    /// <param name="event">رویداد برای ارسال</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task DispatchAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;

    /// <summary>
    /// ارسال چندین رویداد دامنه به صورت دسته‌ای
    /// </summary>
    /// <param name="events">لیست رویدادها برای ارسال</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task DispatchBatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);

    /// <summary>
    /// ثبت یک مصرف‌کننده برای رویداد خاص
    /// </summary>
    /// <typeparam name="TEvent">نوع رویداد</typeparam>
    /// <param name="handler">مصرف‌کننده رویداد</param>
    void RegisterHandler<TEvent>(IDomainEventHandler<TEvent> handler)
        where TEvent : IDomainEvent;

    /// <summary>
    /// حذف یک مصرف‌کننده از رویداد خاص
    /// </summary>
    /// <typeparam name="TEvent">نوع رویداد</typeparam>
    /// <param name="handler">مصرف‌کننده رویداد</param>
    void UnregisterHandler<TEvent>(IDomainEventHandler<TEvent> handler)
        where TEvent : IDomainEvent;

    /// <summary>
    /// دریافت لیست مصرف‌کننده‌ها برای یک رویداد خاص
    /// </summary>
    /// <typeparam name="TEvent">نوع رویداد</typeparam>
    /// <returns>لیست مصرف‌کننده‌ها</returns>
    IEnumerable<IDomainEventHandler<TEvent>> GetHandlers<TEvent>()
        where TEvent : IDomainEvent;
}

/// <summary>
/// اینترفیس برای مصرف‌کننده‌های رویدادهای دامنه
/// </summary>
/// <typeparam name="TEvent">نوع رویداد</typeparam>
public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    /// <summary>
    /// پردازش رویداد
    /// </summary>
    /// <param name="event">رویداد برای پردازش</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}

/// <summary>
/// کلاس پایه برای مصرف‌کننده‌های رویدادهای دامنه
/// </summary>
/// <typeparam name="TEvent">نوع رویداد</typeparam>
public abstract class DomainEventHandlerBase<TEvent> : IDomainEventHandler<TEvent>
    where TEvent : IDomainEvent
{
    /// <summary>
    /// پردازش رویداد
    /// </summary>
    public abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}