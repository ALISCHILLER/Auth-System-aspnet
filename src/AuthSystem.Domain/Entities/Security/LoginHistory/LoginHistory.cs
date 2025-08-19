using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.Security.LoginHistory;

/// <summary>
/// Aggregate Root برای تاریخچه ورود
/// این کلاس مسئول مدیریت تاریخچه ورود کاربران است
/// </summary>
public class LoginHistory : AggregateRoot<Guid>
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
    /// تاریخ ایجاد
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// آخرین زمان به‌روزرسانی
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// تاریخ آخرین ورود موفق
    /// </summary>
    public DateTime? LastSuccessfulLogin { get; private set; }

    /// <summary>
    /// تاریخ آخرین ورود ناموفق
    /// </summary>
    public DateTime? LastFailedLogin { get; private set; }

    /// <summary>
    /// تعداد کل ورودهای موفق
    /// </summary>
    public int SuccessfulLoginCount { get; private set; }

    /// <summary>
    /// تعداد کل ورودهای ناموفق
    /// </summary>
    public int FailedLoginCount { get; private set; }

    /// <summary>
    /// تاریخ قفل شدن حساب (در صورت وجود)
    /// </summary>
    public DateTime? AccountLockedUntil { get; private set; }

    /// <summary>
    /// دلیل قفل شدن
    /// </summary>
    public string? LockReason { get; private set; }

    /// <summary>
    /// تعداد تلاش‌های ناموفق پیاپی
    /// </summary>
    public int ConsecutiveFailedAttempts { get; private set; }

    /// <summary>
    /// ورودهای موفق
    /// </summary>
    private readonly List<LoginAttempt> _successfulLogins = new();
    public IReadOnlyList<LoginAttempt> SuccessfulLogins => _successfulLogins.AsReadOnly();

    /// <summary>
    /// تلاش‌های ناموفق
    /// </summary>
    private readonly List<FailedLoginAttempt> _failedLogins = new();
    public IReadOnlyList<FailedLoginAttempt> FailedLogins => _failedLogins.AsReadOnly();

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private LoginHistory()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    public LoginHistory(
        Guid id,
        Guid userId,
        string username) : base(id)
    {
        UserId = userId;
        Username = username;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        SuccessfulLoginCount = 0;
        FailedLoginCount = 0;
        ConsecutiveFailedAttempts = 0;
    }

    /// <summary>
    /// ثبت ورود موفق
    /// </summary>
    public LoginAttempt RecordSuccessfulLogin(
        IpAddress ipAddress,
        UserAgent userAgent,
        DateTime? loginTime = null)
    {
        var loginTimeValue = loginTime ?? DateTime.UtcNow;

        var login = new LoginAttempt(
            Guid.NewGuid(),
            Id,
            UserId,
            Username,
            ipAddress,
            userAgent,
            loginTimeValue);

        _successfulLogins.Add(login);
        SuccessfulLoginCount++;
        LastSuccessfulLogin = loginTimeValue;
        ConsecutiveFailedAttempts = 0;
        UpdatedAt = loginTimeValue;

        // اگر حساب قفل شده بود، بازگرداندن
        if (AccountLockedUntil.HasValue && AccountLockedUntil.Value <= loginTimeValue)
        {
            UnlockAccount();
        }

        return login;
    }

    /// <summary>
    /// ثبت ورود ناموفق
    /// </summary>
    public FailedLoginAttempt RecordFailedLogin(
        IpAddress ipAddress,
        UserAgent userAgent,
        string failureReason,
        DateTime? loginTime = null)
    {
        var loginTimeValue = loginTime ?? DateTime.UtcNow;

        var login = new FailedLoginAttempt(
            Guid.NewGuid(),
            UserId,
            Username,
            ipAddress,
            userAgent,
            failureReason);

        _failedLogins.Add(login);
        FailedLoginCount++;
        LastFailedLogin = loginTimeValue;
        ConsecutiveFailedAttempts++;
        UpdatedAt = loginTimeValue;

        return login;
    }

    /// <summary>
    /// قفل کردن حساب
    /// </summary>
    public void LockAccount(DateTime lockUntil, string reason)
    {
        AccountLockedUntil = lockUntil;
        LockReason = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// باز کردن قفل حساب
    /// </summary>
    public void UnlockAccount()
    {
        AccountLockedUntil = null;
        LockReason = null;
        ConsecutiveFailedAttempts = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// آیا حساب قفل شده است
    /// </summary>
    public bool IsAccountLocked()
    {
        return AccountLockedUntil.HasValue &&
               AccountLockedUntil.Value > DateTime.UtcNow;
    }

    /// <summary>
    /// دریافت تلاش‌های ناموفق اخیر
    /// </summary>
    public IReadOnlyList<FailedLoginAttempt> GetRecentFailedLogins(TimeSpan timeSpan)
    {
        var cutoff = DateTime.UtcNow - timeSpan;
        return _failedLogins
            .Where(f => f.AttemptTime >= cutoff)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// دریافت ورودهای موفق اخیر
    /// </summary>
    public IReadOnlyList<LoginAttempt> GetRecentSuccessfulLogins(TimeSpan timeSpan)
    {
        var cutoff = DateTime.UtcNow - timeSpan;
        return _successfulLogins
            .Where(s => s.LoginTime >= cutoff)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// تعداد کل ورودهای اخیر
    /// </summary>
    public int GetTotalLogins(TimeSpan timeSpan)
    {
        var cutoff = DateTime.UtcNow - timeSpan;
        return _successfulLogins.Count(s => s.LoginTime >= cutoff) +
               _failedLogins.Count(f => f.AttemptTime >= cutoff);
    }

    /// <summary>
    /// تاریخ اولین ورود
    /// </summary>
    public DateTime? GetFirstLoginDate()
    {
        if (!_successfulLogins.Any())
            return null;

        return _successfulLogins.Min(l => l.LoginTime);
    }

    /// <summary>
    /// تاریخ اولین ورود ناموفق
    /// </summary>
    public DateTime? GetFirstFailedLoginDate()
    {
        if (!_failedLogins.Any())
            return null;

        return _failedLogins.Min(l => l.AttemptTime);
    }

    /// <summary>
    /// تاریخ آخرین ورود از دستگاه خاص
    /// </summary>
    public DateTime? GetLastLoginFromDevice(UserAgent userAgent)
    {
        var login = _successfulLogins
            .Where(l => l.UserAgent.Value == userAgent.Value)
            .OrderByDescending(l => l.LoginTime)
            .FirstOrDefault();

        return login?.LoginTime;
    }

    /// <summary>
    /// تاریخ آخرین ورود از IP خاص
    /// </summary>
    public DateTime? GetLastLoginFromIp(IpAddress ipAddress)
    {
        var login = _successfulLogins
            .Where(l => l.IpAddress.Value == ipAddress.Value)
            .OrderByDescending(l => l.LoginTime)
            .FirstOrDefault();

        return login?.LoginTime;
    }

    /// <summary>
    /// تأیید صحت تاریخچه ورود
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new DomainException("نام کاربری نمی‌تواند خالی باشد");

        if (SuccessfulLoginCount < 0)
            throw new DomainException("تعداد ورودهای موفق نمی‌تواند منفی باشد");

        if (FailedLoginCount < 0)
            throw new DomainException("تعداد ورودهای ناموفق نمی‌تواند منفی باشد");

        if (ConsecutiveFailedAttempts < 0)
            throw new DomainException("تعداد تلاش‌های ناموفق پیاپی نمی‌تواند منفی باشد");
    }

    /// <summary>
    /// محاسبه نسبت موفقیت
    /// </summary>
    public double CalculateSuccessRate()
    {
        if (SuccessfulLoginCount + FailedLoginCount == 0)
            return 0;

        return (double)SuccessfulLoginCount / (SuccessfulLoginCount + FailedLoginCount) * 100;
    }

    /// <summary>
    /// آیا الگوی ورود مشکوک دارد
    /// </summary>
    public bool HasSuspiciousLoginPattern()
    {
        // بررسی الگوهای مشکوک:
        // 1. تلاش‌های متعدد ناموفق در زمان کوتاه
        if (ConsecutiveFailedAttempts >= 5)
            return true;

        // 2. ورود از چندین IP مختلف در زمان کوتاه
        var recentLogins = GetRecentSuccessfulLogins(TimeSpan.FromHours(1));
        var distinctIps = recentLogins.Select(l => l.IpAddress.Value).Distinct();
        if (distinctIps.Count() >= 3 && recentLogins.Count >= 5)
            return true;

        // 3. ورود در ساعات غیرمعمول
        var lastLogin = recentLogins.OrderByDescending(l => l.LoginTime).FirstOrDefault();
        if (lastLogin != null && (lastLogin.LoginTime.Hour < 3 || lastLogin.LoginTime.Hour > 5))
            return true;

        return false;
    }
}