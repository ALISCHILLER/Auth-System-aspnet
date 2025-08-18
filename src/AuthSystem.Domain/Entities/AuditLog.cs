using System;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت لاگ عملیات (Audit Log)
/// برای ردیابی تغییرات مهم در سیستم
/// </summary>
public class AuditLog : BaseEntity<Guid>
{
    /// <summary>
    /// نوع رویداد
    /// </summary>
    public string EventType { get; private set; } = null!;

    /// <summary>
    /// شناسه کاربر (در صورت وجود)
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// شناسه موجودیت مورد تغییر
    /// </summary>
    public string? EntityId { get; private set; }

    /// <summary>
    /// نوع موجودیت
    /// </summary>
    public string? EntityType { get; private set; }

    /// <summary>
    /// آدرس IP انجام دهنده عملیات
    /// </summary>
    public IpAddress IpAddress { get; private set; }

    /// <summary>
    /// User Agent
    /// </summary>
    public UserAgent UserAgent { get; private set; }

    /// <summary>
    /// توضیحات عملیات
    /// </summary>
    public string Description { get; private set; } = null!;

    /// <summary>
    /// جزئیات اضافی (به صورت JSON)
    /// </summary>
    public string? Details { get; private set; }

    /// <summary>
    /// سطح اهمیت
    /// </summary>
    public AuditLogLevel Level { get; private set; }

    /// <summary>
    /// زمان ثبت
    /// </summary>
    public DateTime Timestamp { get; private set; }

    /// <summary>
    /// سازنده خصوصی برای ORM
    /// </summary>
    private AuditLog() { }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    private AuditLog(
        string eventType,
        IpAddress ipAddress,
        UserAgent userAgent,
        string description,
        AuditLogLevel level = AuditLogLevel.Info)
    {
        Id = Guid.NewGuid();
        EventType = eventType;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Description = description;
        Level = level;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// ایجاد نمونه جدید لاگ
    /// </summary>
    public static AuditLog Create(
        string eventType,
        IpAddress ipAddress,
        UserAgent userAgent,
        string description,
        Guid? userId = null,
        string? entityId = null,
        string? entityType = null,
        string? details = null,
        AuditLogLevel level = AuditLogLevel.Info)
    {
        var log = new AuditLog(eventType, ipAddress, userAgent, description, level)
        {
            UserId = userId,
            EntityId = entityId,
            EntityType = entityType,
            Details = details
        };
        return log;
    }

    /// <summary>
    /// نشانه‌گذاری به عنوان حساس
    /// </summary>
    public void MarkAsSensitive()
    {
        Level = AuditLogLevel.Sensitive;
    }

    /// <summary>
    /// نشانه‌گذاری به عنوان خطا
    /// </summary>
    public void MarkAsError()
    {
        Level = AuditLogLevel.Error;
    }
}

/// <summary>
/// سطح اهمیت لاگ
/// </summary>
public enum AuditLogLevel
{
    Debug,
    Info,
    Warning,
    Error,
    Sensitive,  // برای عملیات حساس
    Critical
}