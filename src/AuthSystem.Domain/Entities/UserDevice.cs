using AuthSystem.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت دستگاه کاربر
/// این کلاس برای مدیریت دستگاه‌هایی که کاربر از آن‌ها وارد شده است استفاده می‌شود
/// </summary>
public class UserDevice : BaseEntity
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// کاربر مربوطه
    /// </summary>
    public User User { get; private set; } = default!;

    /// <summary>
    /// شناسه یکتای دستگاه (Fingerprint یا Device Identifier)
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string DeviceId { get; private set; } = string.Empty;

    /// <summary>
    /// نام دستگاه (اختیاری)
    /// </summary>
    [MaxLength(100)]
    public string? DeviceName { get; private set; }

    /// <summary>
    /// نوع دستگاه
    /// </summary>
    public DeviceType DeviceType { get; private set; } = DeviceType.Unknown;

    /// <summary>
    /// نام سیستم عامل
    /// </summary>
    [MaxLength(50)]
    public string? OsName { get; private set; }

    /// <summary>
    /// اطلاعات مرورگر
    /// </summary>
    [MaxLength(255)]
    public string? BrowserInfo { get; private set; }

    /// <summary>
    /// آخرین آدرس IP مورد استفاده از این دستگاه
    /// </summary>
    public string? IpAddress { get; private set; }

    /// <summary>
    /// تاریخ آخرین ورود از این دستگاه
    /// </summary>
    public DateTime LastLoginAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// آیا این دستگاه مورد اعتماد است؟
    /// </summary>
    public bool IsTrusted { get; private set; } = false;

    // متدهای دامنه‌ای

    /// <summary>
    /// ساخت یک دستگاه کاربر جدید
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="deviceId">شناسه دستگاه</param>
    /// <param name="deviceType">نوع دستگاه</param>
    /// <param name="ipAddress">آدرس IP</param>
    /// <param name="userAgent">اطلاعات User Agent</param>
    /// <returns>یک نمونه جدید از کلاس UserDevice</returns>
    public static UserDevice Create(Guid userId, string deviceId, DeviceType deviceType, string? ipAddress, string? userAgent)
    {
        return new UserDevice
        {
            UserId = userId,
            DeviceId = deviceId,
            DeviceType = deviceType,
            IpAddress = ipAddress,
            LastLoginAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// به‌روزرسانی اطلاعات دستگاه
    /// </summary>
    /// <param name="deviceName">نام دستگاه</param>
    /// <param name="osName">نام سیستم عامل</param>
    /// <param name="browserInfo">اطلاعات مرورگر</param>
    /// <param name="ipAddress">آدرس IP</param>
    public void UpdateDeviceInfo(string? deviceName, string? osName, string? browserInfo, string? ipAddress)
    {
        DeviceName = deviceName;
        OsName = osName;
        BrowserInfo = browserInfo;
        IpAddress = ipAddress;
        LastLoginAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    /// <summary>
    /// علامت‌گذاری دستگاه به عنوان مورد اعتماد
    /// </summary>
    public void MarkAsTrusted()
    {
        IsTrusted = true;
        MarkAsUpdated();
    }

    /// <summary>
    /// علامت‌گذاری دستگاه به عنوان غیرقابل اعتماد
    /// </summary>
    public void MarkAsUntrusted()
    {
        IsTrusted = false;
        MarkAsUpdated();
    }
}