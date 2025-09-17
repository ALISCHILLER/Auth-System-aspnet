using System;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.AuditLog;

/// <summary>
/// Entity برای جزئیات لاگ حسابرسی
/// </summary>
public class AuditLogEntry : BaseEntity<Guid>
{
    /// <summary>
    /// شناسه لاگ والد
    /// </summary>
    public Guid LogId { get; }

    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// نام کاربری
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// عملیات انجام شده
    /// </summary>
    public string Action { get; }

    /// <summary>
    /// توضیحات عملیات
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// سطح اهمیت
    /// </summary>
    public AuditLogLevel LogLevel { get; }

    /// <summary>
    /// آدرس IP
    /// </summary>
    public IpAddress IpAddress { get; }

    /// <summary>
    /// User Agent
    /// </summary>
    public UserAgent UserAgent { get; }

    /// <summary>
    /// تاریخ و زمان
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private AuditLogEntry()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    private AuditLogEntry(
        Guid id,
        Guid logId,
        Guid userId,
        string username,
        string action,
        string description,
        AuditLogLevel logLevel,
        IpAddress ipAddress,
        UserAgent userAgent,
        DateTime timestamp) : base(id)
    {
        LogId = logId;
        UserId = userId;
        Username = username;
        Action = action;
        Description = description;
        LogLevel = logLevel;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Timestamp = timestamp;
    }

    /// <summary>
    /// ایجاد جزئیات جدید
    /// </summary>
    public static AuditLogEntry Create(
        Guid id,
        Guid logId,
        Guid userId,
        string username,
        string action,
        string description,
        AuditLogLevel logLevel,
        IpAddress ipAddress,
        UserAgent userAgent,
        DateTime timestamp)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidAuditLogEntryException(id, "نام کاربری نمی‌تواند خالی باشد");

        if (string.IsNullOrWhiteSpace(action))
            throw new InvalidAuditLogEntryException(id, "عملیات نمی‌تواند خالی باشد");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidAuditLogEntryException(id, "توضیحات نمی‌تواند خالی باشد");

        if (ipAddress == null)
            throw new InvalidAuditLogEntryException(id, "آدرس IP نمی‌تواند خالی باشد");

        if (userAgent == null)
            throw new InvalidAuditLogEntryException(id, "User Agent نمی‌تواند خالی باشد");

        return new AuditLogEntry(
            id,
            logId,
            userId,
            username,
            action,
            description,
            logLevel,
            ipAddress,
            userAgent,
            timestamp);
    }

    /// <summary>
    /// تأیید صحت جزئیات
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new InvalidAuditLogEntryException(Id, "نام کاربری نمی‌تواند خالی باشد");

        if (string.IsNullOrWhiteSpace(Action))
            throw new InvalidAuditLogEntryException(Id, "عملیات نمی‌تواند خالی باشد");

        if (string.IsNullOrWhiteSpace(Description))
            throw new InvalidAuditLogEntryException(Id, "توضیحات نمی‌تواند خالی باشد");

        if (IpAddress == null)
            throw new InvalidAuditLogEntryException(Id, "آدرس IP نمی‌تواند خالی باشد");

        if (UserAgent == null)
            throw new InvalidAuditLogEntryException(Id, "User Agent نمی‌تواند خالی باشد");
    }

    /// <summary>
    /// آیا این جزئیات از نوع خاصی است
    /// </summary>
    public bool IsOfType(AuditLogLevel level)
    {
        return LogLevel == level;
    }

    /// <summary>
    /// آیا این جزئیات مربوط به کاربر خاصی است
    /// </summary>
    public bool IsForUser(Guid userId)
    {
        return UserId == userId;
    }

    /// <summary>
    /// آیا این جزئیات مربوط به عملیات خاصی است
    /// </summary>
    public bool IsForAction(string action)
    {
        return Action.Equals(action, StringComparison.OrdinalIgnoreCase);
    }
}