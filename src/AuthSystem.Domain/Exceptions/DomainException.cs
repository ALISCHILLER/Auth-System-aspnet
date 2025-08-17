using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// کلاس پایه برای تمام استثناهای دامنه
/// </summary>
[Serializable]
public abstract class DomainException : Exception
{
    /// <summary>
    /// کد خطا برای شناسایی نوع خطا در سیستم
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// جزئیات اضافی خطا
    /// </summary>
    public IDictionary<string, object> Details { get; }

    /// <summary>
    /// زمان وقوع خطا
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// شناسه یکتا برای ردیابی خطا
    /// </summary>
    public string TraceId { get; }

    protected DomainException(string message, string errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
        Details = new Dictionary<string, object>();
        OccurredOn = DateTime.UtcNow;
        TraceId = Guid.NewGuid().ToString("N");
    }

    protected DomainException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Details = new Dictionary<string, object>();
        OccurredOn = DateTime.UtcNow;
        TraceId = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// اضافه کردن جزئیات به خطا (Fluent API)
    /// </summary>
    public DomainException WithDetail(string key, object value)
    {
        Details[key] = value;
        return this;
    }

    /// <summary>
    /// اضافه کردن چندین جزئیات
    /// </summary>
    public DomainException WithDetails(IDictionary<string, object> details)
    {
        foreach (var detail in details)
        {
            Details[detail.Key] = detail.Value;
        }
        return this;
    }

    /// <summary>
    /// سریال‌سازی برای انتقال بین AppDomains
    /// </summary>
    protected DomainException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ErrorCode = info.GetString(nameof(ErrorCode)) ?? string.Empty;
        OccurredOn = info.GetDateTime(nameof(OccurredOn));
        TraceId = info.GetString(nameof(TraceId)) ?? Guid.NewGuid().ToString("N");

        try
        {
            Details = (IDictionary<string, object>)info.GetValue(nameof(Details), typeof(IDictionary<string, object>))
                      ?? new Dictionary<string, object>();
        }
        catch
        {
            Details = new Dictionary<string, object>();
        }
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ErrorCode), ErrorCode);
        info.AddValue(nameof(OccurredOn), OccurredOn);
        info.AddValue(nameof(TraceId), TraceId);
        info.AddValue(nameof(Details), Details);
    }
}