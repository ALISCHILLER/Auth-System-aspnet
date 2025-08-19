using System;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.Security.LoginHistory;

/// <summary>
/// Entity برای تلاش‌های ناموفق ورود
/// </summary>
public class FailedLoginAttempt : BaseEntity<Guid>
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// نام کاربری
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// آدرس IP
    /// </summary>
    public IpAddress IpAddress { get; }

    /// <summary>
    /// User Agent
    /// </summary>
    public UserAgent UserAgent { get; }

    /// <summary>
    /// زمان تلاش
    /// </summary>
    public DateTime AttemptTime { get; }

    /// <summary>
    /// دلیل شکست
    /// </summary>
    public string FailureReason { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private FailedLoginAttempt()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    public FailedLoginAttempt(
        Guid id,
        Guid userId,
        string username,
        IpAddress ipAddress,
        UserAgent userAgent,
        string failureReason) : base(id)
    {
        UserId = userId;
        Username = username;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        AttemptTime = DateTime.UtcNow;
        FailureReason = failureReason;
    }

    /// <summary>
    /// آیا این تلاش مربوط به کاربر خاصی است
    /// </summary>
    public bool IsForUser(Guid userId)
    {
        return UserId == userId;
    }

    /// <summary>
    /// آیا این تلاش مربوط به آدرس IP خاصی است
    /// </summary>
    public bool IsFromIpAddress(IpAddress ipAddress)
    {
        return IpAddress.Value == ipAddress.Value;
    }

    /// <summary>
    /// آیا این تلاش اخیر است
    /// </summary>
    public bool IsRecent(TimeSpan timeSpan)
    {
        return DateTime.UtcNow - AttemptTime <= timeSpan;
    }
}