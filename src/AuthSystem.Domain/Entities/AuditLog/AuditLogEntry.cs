using System;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.AuditLog;

/// <summary>
/// Entity representing an individual audit log entry.
/// </summary>
public class AuditLogEntry : BaseEntity<Guid>
{
    public Guid LogId { get; private set; }
    public Guid UserId { get; private set; }
    public string Username { get; private set; } = default!;
    public string Action { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public AuditLogLevel LogLevel { get; private set; }
    public IpAddress IpAddress { get; private set; } = default!;
    public UserAgent UserAgent { get; private set; } = default!;
    public DateTime Timestamp { get; private set; }

    private AuditLogEntry()
    {
        
    }

    
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
        Timestamp = NormalizeTimestamp(timestamp);
        MarkAsCreated(occurredOn: Timestamp);
    }

  
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
        {
            throw new InvalidAuditLogEntryException(id, "نام کاربری نمی‌تواند خالی باشد");
        }

        if (string.IsNullOrWhiteSpace(action))
        {
            throw new InvalidAuditLogEntryException(id, "عملیات نمی‌تواند خالی باشد");
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidAuditLogEntryException(id, "توضیحات نمی‌تواند خالی باشد");
        }

        if (ipAddress is null)
        {
            throw new InvalidAuditLogEntryException(id, "آدرس IP نمی‌تواند خالی باشد");
        }

        if (userAgent is null)
        {
            throw new InvalidAuditLogEntryException(id, "User Agent نمی‌تواند خالی باشد");
        }

        var occurredAt = NormalizeTimestamp(timestamp);
        return new AuditLogEntry(id, logId, userId, username, action, description, logLevel, ipAddress, userAgent, occurredAt);
    }

 
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            throw new InvalidAuditLogEntryException(Id, "نام کاربری نمی‌تواند خالی باشد");
        }

        if (string.IsNullOrWhiteSpace(Action))
        {
            throw new InvalidAuditLogEntryException(Id, "عملیات نمی‌تواند خالی باشد");
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            throw new InvalidAuditLogEntryException(Id, "توضیحات نمی‌تواند خالی باشد");
        }

        if (IpAddress is null)
        {
            throw new InvalidAuditLogEntryException(Id, "آدرس IP نمی‌تواند خالی باشد");
        }
        if (UserAgent is null)
        {
            throw new InvalidAuditLogEntryException(Id, "User Agent نمی‌تواند خالی باشد");
        }
    }

    public bool IsOfType(AuditLogLevel level) => LogLevel == level;
    public bool IsForUser(Guid userId) => UserId == userId;
    public bool IsForAction(string action) => Action.Equals(action, StringComparison.OrdinalIgnoreCase);

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