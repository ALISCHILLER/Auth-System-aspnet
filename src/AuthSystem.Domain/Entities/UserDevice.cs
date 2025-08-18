using System;
using AuthSystem.Domain.Common.Auditing;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت دستگاه کاربر
/// برای مدیریت دستگاه‌های مورد اعتماد کاربر
/// </summary>
public class UserDevice : BaseEntity<Guid>, IAuditableEntity
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// شناسه منحصر به فرد دستگاه
    /// </summary>
    public string DeviceId { get; private set; } = null!;

    /// <summary>
    /// نام دستگاه (مثلاً "گوشی شخصی" یا "لپ‌تاپ کار")
    /// </summary>
    public string DeviceName { get; private set; } = null!;

    /// <summary>
    /// نوع دستگاه
    /// </summary>
    public DeviceType Type { get; private set; }

    /// <summary>
    /// آیا دستگاه مورد اعتماد است
    /// </summary>
    public bool IsTrusted { get; private set; }

    /// <summary>
    /// زمان آخرین استفاده
    /// </summary>
    public DateTime LastUsedAt { get; private set; }

    /// <summary>
    /// تاریخ ایجاد (از IAuditableEntity)
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// تاریخ آخرین به‌روزرسانی (از IAuditableEntity)
    /// </summary>
    public DateTime? UpdatedAt => LastUsedAt;

    /// <summary>
    /// سازنده خصوصی برای ORM
    /// </summary>
    private UserDevice() { }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    private UserDevice(
        Guid userId,
        string deviceId,
        string deviceName,
        DeviceType type)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        DeviceId = deviceId;
        DeviceName = deviceName;
        Type = type;
        IsTrusted = false;
        LastUsedAt = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// ایجاد نمونه جدید دستگاه کاربر
    /// </summary>
    public static UserDevice Create(
        Guid userId,
        string deviceId,
        string deviceName,
        DeviceType type)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException("شناسه دستگاه نمی‌تواند خالی باشد", nameof(deviceId));

        if (string.IsNullOrWhiteSpace(deviceName))
            throw new ArgumentException("نام دستگاه نمی‌تواند خالی باشد", nameof(deviceName));

        return new UserDevice(userId, deviceId, deviceName, type);
    }

    /// <summary>
    /// اعتماد به دستگاه
    /// </summary>
    public void Trust()
    {
        IsTrusted = true;
        LastUsedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// عدم اعتماد به دستگاه
    /// </summary>
    public void Untrust()
    {
        IsTrusted = false;
        LastUsedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// ثبت استفاده از دستگاه
    /// </summary>
    public void RecordUsage()
    {
        LastUsedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// تغییر نام دستگاه
    /// </summary>
    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("نام جدید نمی‌تواند خالی باشد");

        DeviceName = newName;
        LastUsedAt = DateTime.UtcNow;
    }
}