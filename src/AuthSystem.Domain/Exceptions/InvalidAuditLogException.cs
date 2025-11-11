using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای لاگ عملیات نامعتبر
/// این استثنا زمانی رخ می‌دهد که ساختار کلی لاگ عملیات نامعتبر باشد
/// </summary>
public class InvalidAuditLogException : DomainException
{
    /// <summary>
    /// شناسه لاگ
    /// </summary>
    public Guid? LogId { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidAuditLog";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidAuditLogException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidAuditLogException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با شناسه لاگ و پیام خطا
    /// </summary>
    public InvalidAuditLogException(Guid logId, string message)
        : this(message)
    {
        LogId = logId;
    }

    /// <summary>
    /// ایجاد استثنا برای لاگ عملیات نامعتبر
    /// </summary>
    public static InvalidAuditLogException ForInvalidLog(string reason)
    {
        return new InvalidAuditLogException($"لاگ عملیات نامعتبر است: {reason}");
    }

    /// <summary>
    /// ایجاد استثنا برای لاگ عملیات نامعتبر با شناسه
    /// </summary>
    public static InvalidAuditLogException ForInvalidLog(Guid logId, string reason)
    {
        return new InvalidAuditLogException(logId, $"لاگ عملیات با شناسه {logId} نامعتبر است: {reason}");
    }

    /// <summary>
    /// ایجاد استثنا برای لاگ عملیات فاقد سطح اهمیت
    /// </summary>
    public static InvalidAuditLogException ForMissingLogLevel()
    {
        return new InvalidAuditLogException("لاگ عملیات فاقد سطح اهمیت است");
    }

    /// <summary>
    /// ایجاد استثنا برای لاگ عملیات فاقد تاریخ
    /// </summary>
    public static InvalidAuditLogException ForMissingTimestamp()
    {
        return new InvalidAuditLogException("لاگ عملیات فاقد تاریخ و زمان است");
    }
}