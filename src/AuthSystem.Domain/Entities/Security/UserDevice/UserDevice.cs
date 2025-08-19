using System;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.Security.UserDevice;

/// <summary>
/// Entity برای دستگاه‌های کاربر
/// </summary>
public class UserDevice : BaseEntity<Guid>
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
    /// نام دستگاه
    /// </summary>
    public string DeviceName { get; private set; }

    /// <summary>
    /// نوع دستگاه
    /// </summary>
    public DeviceType DeviceType { get; }

    /// <summary>
    /// User Agent
    /// </summary>
    public UserAgent UserAgent { get; }

    /// <summary>
    /// آدرس IP
    /// </summary>
    public IpAddress IpAddress { get; }

    /// <summary>
    /// کلید عمومی (برای احراز هویت)
    /// </summary>
    public string PublicKey { get; }

    /// <summary>
    /// تاریخ ایجاد
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// تاریخ آخرین استفاده
    /// </summary>
    public DateTime LastUsed { get; private set; }

    /// <summary>
    /// آیا دستگاه تأیید شده است
    /// </summary>
    public bool IsVerified { get; private set; }

    /// <summary>
    /// تاریخ تأیید
    /// </summary>
    public DateTime? VerifiedAt { get; private set; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private UserDevice()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    public UserDevice(
        Guid id,
        Guid userId,
        string username,
        string deviceName,
        DeviceType deviceType,
        UserAgent userAgent,
        IpAddress ipAddress,
        string publicKey) : base(id)
    {
        UserId = userId;
        Username = username;
        UpdateDeviceName(deviceName);
        DeviceType = deviceType;
        UserAgent = userAgent;
        IpAddress = ipAddress;
        PublicKey = publicKey;
        CreatedAt = DateTime.UtcNow;
        LastUsed = CreatedAt;
        IsVerified = false;
        VerifiedAt = null;
    }

    /// <summary>
    /// به‌روزرسانی نام دستگاه
    /// </summary>
    public void UpdateDeviceName(string deviceName)
    {
        if (string.IsNullOrWhiteSpace(deviceName))
            throw new DomainException("نام دستگاه نمی‌تواند خالی باشد");

        if (deviceName.Length > 100)
            throw new DomainException("نام دستگاه نمی‌تواند بیشتر از 100 کاراکتر باشد");

        DeviceName = deviceName;
    }

    /// <summary>
    /// تأیید دستگاه
    /// </summary>
    public void Verify()
    {
        if (IsVerified)
            return;

        IsVerified = true;
        VerifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// لغو تأیید دستگاه
    /// </summary>
    public void Unverify()
    {
        IsVerified = false;
        VerifiedAt = null;
    }

    /// <summary>
    /// به‌روزرسانی آخرین استفاده
    /// </summary>
    public void UpdateLastUsed()
    {
        LastUsed = DateTime.UtcNow;
    }

    /// <summary>
    /// آیا دستگاه مربوط به کاربر خاصی است
    /// </summary>
    public bool IsForUser(Guid userId)
    {
        return UserId == userId;
    }

    /// <summary>
    /// آیا دستگاه تأیید شده است
    /// </summary>
    public bool IsTrusted()
    {
        return IsVerified;
    }

    /// <summary>
    /// آیا دستگاه اخیراً استفاده شده است
    /// </summary>
    public bool IsRecentlyUsed(TimeSpan timeSpan)
    {
        return DateTime.UtcNow - LastUsed <= timeSpan;
    }

    /// <summary>
    /// دریافت اطلاعات دستگاه
    /// </summary>
    public string GetDeviceInfo()
    {
        return $"{UserAgent.GetBrowserFullName()} on {UserAgent.GetOsFullName()}";
    }

    /// <summary>
    /// تأیید صحت دستگاه
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new DomainException("نام کاربری نمی‌تواند خالی باشد");

        if (string.IsNullOrWhiteSpace(DeviceName))
            throw new DomainException("نام دستگاه نمی‌تواند خالی باشد");

        if (DeviceName.Length > 100)
            throw new DomainException("نام دستگاه نمی‌تواند بیشتر از 100 کاراکتر باشد");

        if (string.IsNullOrWhiteSpace(PublicKey))
            throw new DomainException("کلید عمومی نمی‌تواند خالی باشد");
    }
}