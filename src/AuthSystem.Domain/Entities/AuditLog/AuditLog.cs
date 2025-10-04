using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.AuditLog;

/// <summary>
/// Aggregate root representing an audit log session.
/// </summary>
public class AuditLog : AggregateRoot<Guid>
{
    private readonly List<AuditLogEntry> _entries = new();

    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }

    public int EntryCount => _entries.Count;

  
    public IReadOnlyList<AuditLogEntry> Entries => _entries.AsReadOnly();

  
    private AuditLog()
    {
       
    }

  
    public AuditLog(Guid id, DateTime? startTime = null) : base(id)
    {
        var startedAt = startTime.HasValue ? NormalizeTimestamp(startTime.Value) : DomainClock.Instance.UtcNow;
        StartTime = startedAt;
        EndTime = null;
        _entries.Clear();
        MarkAsCreated(occurredOn: startedAt);
    }

    public AuditLogEntry AddEntry(
         Guid userId,
        string username,
        string action,
        string description,
        AuditLogLevel logLevel,
        IpAddress ipAddress,
        UserAgent userAgent,
        DateTime? timestamp = null)
    {
        var occurredAt = timestamp.HasValue ? NormalizeTimestamp(timestamp.Value) : DomainClock.Instance.UtcNow;
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
            occurredAt);

        _entries.Add(entry);
        MarkAsUpdated(occurredOn: occurredAt);
        return entry;
    }

   
    public void AddEntries(IEnumerable<AuditLogEntry> entries)
    {
        if (entries is null)
        {
            throw new ArgumentNullException(nameof(entries));
        }
        foreach (var entry in entries)
        {
            if (entry.LogId != Id)
            {
                throw new InvalidAuditLogException("جزئیات لاگ به این لاگ مربوط نمی‌شود");
            }

            _entries.Add(entry);
        }
        if (_entries.Count > 0)
        {
            MarkAsUpdated(occurredOn: _entries.Max(e => e.Timestamp));
        }
    }


    public void Complete()
    {
        if (EndTime.HasValue)
        {
            return;
        }

        EndTime = DomainClock.Instance.UtcNow;
        MarkAsUpdated(occurredOn: EndTime.Value);
    }

    public IReadOnlyList<AuditLogEntry> GetEntriesByLevel(AuditLogLevel logLevel) =>
     _entries.Where(entry => entry.LogLevel == logLevel).ToList().AsReadOnly();

    public IReadOnlyList<AuditLogEntry> GetEntriesByTimeRange(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new InvalidAuditLogException("بازه زمانی نامعتبر است");
        }

        return _entries
            .Where(entry => entry.Timestamp >= start && entry.Timestamp <= end)
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyList<AuditLogEntry> GetEntriesByUser(Guid userId) =>
        _entries.Where(entry => entry.UserId == userId).ToList().AsReadOnly();

    public IReadOnlyList<AuditLogEntry> GetEntriesByIpAddress(IpAddress ipAddress) =>
          _entries.Where(entry => entry.IpAddress.Value == ipAddress.Value).ToList().AsReadOnly();

    public IReadOnlyList<AuditLogEntry> GetEntriesByAction(string action) =>
        _entries.Where(entry => entry.Action.Equals(action, StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();

   
    public void Validate()
    {
        if (StartTime == default)
        {
            throw new InvalidAuditLogException("تاریخ شروع نمی‌تواند خالی باشد");
        }

        if (EndTime.HasValue && EndTime.Value < StartTime)
        {
            throw new InvalidAuditLogException("تاریخ پایان نمی‌تواند قبل از تاریخ شروع باشد");
        }

        foreach (var entry in _entries)
        {
            entry.Validate();
        }
    }

    
    public TimeSpan? GetDuration()
    {
        if (!EndTime.HasValue)
        {
            return null;
        }

        return EndTime.Value - StartTime;
    }

   
    public void CleanupOldEntries(DateTime cutoffDate)
    {
        _entries.RemoveAll(entry => entry.Timestamp < cutoffDate);
    }

    private static DateTime NormalizeTimestamp(DateTime timestamp)
    {
        return timestamp.Kind switch
        {
            DateTimeKind.Utc => timestamp,
            DateTimeKind.Local => timestamp.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(timestamp, DateTimeKind.Utc),
            _ => timestamp
        };
    }
}