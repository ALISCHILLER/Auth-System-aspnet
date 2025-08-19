using System;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.Security.LoginHistory;

/// <summary>
/// Entity برای ورود موفق
/// </summary>
public class LoginAttempt : BaseEntity<Guid>
{
    /// <summary>
    /// شناسه تاریخچه ورود
    /// </summary>
    public Guid HistoryId { get; }

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
    /// زمان ورود
    /// </summary>
    public DateTime LoginTime { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private LoginAttempt()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    public LoginAttempt(
        Guid id,
        Guid historyId,
        Guid userId,
        string username,
        IpAddress ipAddress,
        UserAgent userAgent,
        DateTime loginTime) : base(id)
    {
        HistoryId = historyId;
        UserId = userId;
        Username = username;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        LoginTime = loginTime;
    }

    /// <summary>
    /// آیا این ورود مربوط به کاربر خاصی است
    /// </summary>
    public bool IsForUser(Guid userId)
    {
        return UserId == userId;
    }

    /// <summary>
    /// آیا این ورود مربوط به تاریخچه خاصی است
    /// </summary>
    public bool IsForHistory(Guid historyId)
    {
        return HistoryId == historyId;
    }

    /// <summary>
    /// آیا این ورود اخیر است
    /// </summary>
    public bool IsRecent(TimeSpan timeSpan)
    {
        return DateTime.UtcNow - LoginTime <= timeSpan;
    }
}