using AuthSystem.Domain.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت مجوز در سیستم احراز هویت.
/// این کلاس نماینده‌ی مجوزهایی مانند CreateUser یا DeleteUser است.
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// نام مجوز (باید یکتا باشد).
    /// محدودیت طول: حداکثر 100 کاراکتر.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// توضیحات مربوط به مجوز.
    /// محدودیت طول: حداکثر 255 کاراکتر.
    /// </summary>
    [MaxLength(255)]
    public string? Description { get; private set; }

    /// <summary>
    /// وضعیت فعال یا غیرفعال بودن مجوز.
    /// مقدار پیش‌فرض: فعال (true).
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// وضعیت حذف منطقی مجوز.
    /// مقدار پیش‌فرض: حذف نشده (false).
    /// </summary>
    public bool IsDeleted { get; private set; } = false;

    /// <summary>
    /// لیست ارتباط بین این مجوز و نقش‌ها (برای رابطه‌ی چند به چند).
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

    /// <summary>
    /// ساخت یک نمونه جدید از مجوز.
    /// </summary>
    /// <param name="name">نام مجوز</param>
    /// <param name="description">توضیحات مجوز (اختیاری)</param>
    /// <returns>نمونه‌ی جدید از کلاس Permission</returns>
    public static Permission Create(string name, string? description = null)
    {
        return new Permission
        {
            Name = name,
            Description = description
        };
    }

    /// <summary>
    /// به‌روزرسانی نام مجوز.
    /// اگر نام خالی باشد، خطا پرتاب می‌شود.
    /// </summary>
    /// <param name="name">نام جدید مجوز</param>
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidPermissionNameException(name);

        Name = name;
        MarkAsUpdated();
    }

    /// <summary>
    /// به‌روزرسانی توضیحات مجوز.
    /// </summary>
    /// <param name="description">توضیحات جدید</param>
    public void UpdateDescription(string? description)
    {
        Description = description;
        MarkAsUpdated();
    }

    /// <summary>
    /// فعال‌سازی مجوز.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        MarkAsUpdated();
    }

    /// <summary>
    /// غیرفعال کردن مجوز.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    /// <summary>
    /// حذف منطقی مجوز (نرم‌حذف).
    /// </summary>
    public void SoftDelete()
    {
        IsDeleted = true;
        MarkAsUpdated();
    }

    /// <summary>
    /// بازگردانی مجوز حذف شده (بازیابی).
    /// </summary>
    public void Restore()
    {
        IsDeleted = false;
        MarkAsUpdated();
    }
}
