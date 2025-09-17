using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.AuditLog;

/// <summary>
/// Aggregate Root برای لاگ‌های حسابرسی
/// این کلاس مسئول مدیریت کلی لاگ‌های حسابرسی است
/// </summary>
public class AuditLog : AggregateRoot<Guid>
{
    /// <summary>
    /// تاریخ و زمان شروع
    /// </summary>
    public DateTime StartTime { get; }

    /// <summary>
    /// تاریخ و زمان پایان
    /// </summary>
    public DateTime? EndTime { get; private set; }

    /// <summary>
    /// تعداد جزئیات لاگ
    /// </summary>
    public int EntryCount => _entries.Count;

    /// <summary>
    /// جزئیات لاگ‌ها
    /// </summary>
    private readonly List<AuditLogEntry> _entries = new();
    public IReadOnlyList<AuditLogEntry> Entries => _entries.AsReadOnly();

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private AuditLog()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    public AuditLog(Guid id, DateTime? startTime = null) : base(id)
    {
        StartTime = startTime ?? DateTime.UtcNow;
        EndTime = null;
    }

    /// <summary>
    /// افزودن جزئیات جدید به لاگ
    /// </summary>
    public void AddEntry(
        Guid userId,
        string username,
        string action,
        string description,
        AuditLogLevel logLevel,
        IpAddress ipAddress,
        UserAgent userAgent,
        DateTime? timestamp = null)
    {
        var entry = AuditLogEntry.Create(
            Guid.NewGuid(),
            Id,
            userId,
            username,
            action,
            description,
            logLevel,
            ipAddress,
            userAgent,
            timestamp ?? DateTime.UtcNow);

        _entries.Add(entry);
    }

    /// <summary>
    /// افزودن چندین جزئیات به لاگ
    /// </summary>
    public void AddEntries(IEnumerable<AuditLogEntry> entries)
    {
        foreach (var entry in entries)
        {
            if (entry.LogId != Id)
                throw new InvalidAuditLogException("جزئیات لاگ به این لاگ مربوط نمی‌شود");

            _entries.Add(entry);
        }
    }

    /// <summary>
    /// پایان‌بندی لاگ
    /// </summary>
    public void Complete()
    {
        EndTime = DateTime.UtcNow;
    }

    /// <summary>
    /// دریافت جزئیات بر اساس سطح اهمیت
    /// </summary>
    public IReadOnlyList<AuditLogEntry> GetEntriesByLevel(AuditLogLevel logLevel)
    {
        return _entries
            .Where(e => e.LogLevel == logLevel)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// دریافت جزئیات بر اساس بازه زمانی
    /// </summary>
    public IReadOnlyList<AuditLogEntry> GetEntriesByTimeRange(DateTime start, DateTime end)
    {
        if (start > end)
            throw new InvalidAuditLogException("بازه زمانی نامعتبر است");

        return _entries
            .Where(e => e.Timestamp >= start && e.Timestamp <= end)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// دریافت جزئیات بر اساس کاربر
    /// </summary>
    public IReadOnlyList<AuditLogEntry> GetEntriesByUser(Guid userId)
    {
        return _entries
            .Where(e => e.UserId == userId)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// دریافت جزئیات بر اساس آدرس IP
    /// </summary>
    public IReadOnlyList<AuditLogEntry> GetEntriesByIpAddress(IpAddress ipAddress)
    {
        return _entries
            .Where(e => e.IpAddress.Value == ipAddress.Value)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// دریافت جزئیات بر اساس عملیات
    /// </summary>
    public IReadOnlyList<AuditLogEntry> GetEntriesByAction(string action)
    {
        return _entries
            .Where(e => e.Action.Equals(action, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// تأیید صحت لاگ
    /// </summary>
    public void Validate()
    {
        if (StartTime == default)
            throw new InvalidAuditLogException("تاریخ شروع نمی‌تواند خالی باشد");

        if (EndTime.HasValue && EndTime.Value < StartTime)
            throw new InvalidAuditLogException("تاریخ پایان نمی‌تواند قبل از تاریخ شروع باشد");

        foreach (var entry in _entries)
        {
            entry.Validate();
        }
    }

    /// <summary>
    /// محاسبه مدت زمان کل
    /// </summary>
    public TimeSpan? GetDuration()
    {
        if (!EndTime.HasValue)
            return null;

        return EndTime.Value - StartTime;
    }

    /// <summary>
    /// پاک کردن جزئیات قدیمی
    /// </summary>
    public void CleanupOldEntries(DateTime cutoffDate)
    {
        _entries.RemoveAll(e => e.Timestamp < cutoffDate);
    }
}