using System;
using System.Collections.Generic;

namespace AuthSystem.Domain.Common.Events;

/// <summary>
/// کلاس برای مدیریت متادیتای رویدادهای دامنه
/// </summary>
public class DomainEventMetadata
{
    /// <summary>
    /// شناسه کاربر انجام‌دهنده عملیات
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// آدرس IP انجام‌دهنده عملیات
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// User Agent انجام‌دهنده عملیات
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// شناسه جلسه کاربر
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// زمان شروع پردازش رویداد
    /// </summary>
    public DateTime ProcessingStartedAt { get; } = DateTime.UtcNow;

    /// <summary>
    /// زمان پایان پردازش رویداد
    /// </summary>
    public DateTime? ProcessingEndedAt { get; private set; }

    /// <summary>
    /// آیا پردازش رویداد موفقیت‌آمیز بود
    /// </summary>
    public bool IsSuccessful { get; private set; }

    /// <summary>
    /// تعداد تلاش‌های پردازش
    /// </summary>
    public int AttemptCount { get; private set; }

    /// <summary>
    /// علامت‌گذاری پردازش به عنوان کامل شده
    /// </summary>
    public void MarkAsCompleted(bool isSuccessful)
    {
        ProcessingEndedAt = DateTime.UtcNow;
        IsSuccessful = isSuccessful;
    }

    /// <summary>
    /// افزودن تلاش جدید برای پردازش
    /// </summary>
    public void IncrementAttemptCount()
    {
        AttemptCount++;
    }

    /// <summary>
    /// محاسبه زمان پردازش
    /// </summary>
    public TimeSpan? GetProcessingDuration()
    {
        if (ProcessingEndedAt == null)
            return null;

        return ProcessingEndedAt.Value - ProcessingStartedAt;
    }
}