using AuthSystem.Domain.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت مجوز در سیستم احراز هویت
/// این کلاس مجوزهای مختلف سیستم را تعریف می‌کند (مثل CreateUser، DeleteUser)
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// نام مجوز (باید منحصر به فرد باشد)
    /// محدودیت طول: حداکثر 100 کاراکتر
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// توضیحات مجوز
    /// محدودیت طول: حداکثر 255 کاراکتر
    /// </summary>
    [MaxLength(255)]
    public string? Description { get; private set; }

    // ویژگی‌های ناوبری (Navigation Properties)

    /// <summary>
    /// لیست روابط مجوز با نقش‌ها (برای رابطه چند به چند)
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

    // متدهای دامنه‌ای

    /// <summary>
    /// ساخت یک مجوز جدید
    /// </summary>
    /// <param name="name">نام مجوز</param>
    /// <param name="description">توضیحات مجوز</param>
    /// <returns>یک نمونه جدید از کلاس Permission</returns>
    public static Permission Create(string name, string? description = null)
    {
        return new Permission
        {
            Name = name,
            Description = description
        };
    }

    /// <summary>
    /// به‌روزرسانی نام مجوز
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
    /// به‌روزرسانی توضیحات مجوز
    /// </summary>
    /// <param name="description">توضیحات جدید مجوز</param>
    public void UpdateDescription(string? description)
    {
        Description = description;
        MarkAsUpdated();
    }
}