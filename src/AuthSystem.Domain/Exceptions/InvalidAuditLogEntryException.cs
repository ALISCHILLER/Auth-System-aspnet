using AuthSystem.Domain.Common.Exceptions;
using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای جزئیات لاگ عملیات نامعتبر
/// این استثنا زمانی رخ می‌دهد که جزئیات ثبت شده در لاگ عملیات نامعتبر باشد
/// </summary>
public class InvalidAuditLogEntryException : DomainException
{
    /// <summary>
    /// شناسه جزئیات لاگ
    /// </summary>
    public Guid? EntryId { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidAuditLogEntry";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidAuditLogEntryException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidAuditLogEntryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با شناسه جزئیات و پیام خطا
    /// </summary>
    public InvalidAuditLogEntryException(Guid entryId, string message)
        : this(message)
    {
        EntryId = entryId;
    }

    /// <summary>
    /// ایجاد استثنا برای جزئیات لاگ نامعتبر
    /// </summary>
    public static InvalidAuditLogEntryException ForInvalidEntry(string reason)
    {
        return new InvalidAuditLogEntryException($"جزئیات لاگ نامعتبر است: {reason}");
    }

    /// <summary>
    /// ایجاد استثنا برای جزئیات لاگ نامعتبر با شناسه
    /// </summary>
    public static InvalidAuditLogEntryException ForInvalidEntry(Guid entryId, string reason)
    {
        return new InvalidAuditLogEntryException(entryId, $"جزئیات لاگ با شناسه {entryId} نامعتبر است: {reason}");
    }

    /// <summary>
    /// ایجاد استثنا برای جزئیات لاگ فاقد داده‌های ضروری
    /// </summary>
    public static InvalidAuditLogEntryException ForMissingRequiredData()
    {
        return new InvalidAuditLogEntryException("جزئیات لاگ فاقد داده‌های ضروری است");
    }
}