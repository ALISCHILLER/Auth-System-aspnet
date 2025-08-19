using AuthSystem.Domain.Common.Exceptions;
using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای نقش کاربر نامعتبر
/// این استثنا زمانی رخ می‌دهد که نقش کاربر از قوانین سیستم پیروی نکند
/// </summary>
public class InvalidUserRoleException : DomainException
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid? UserId { get; }

    /// <summary>
    /// نام نقش
    /// </summary>
    public string RoleName { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidUserRole";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidUserRoleException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidUserRoleException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با شناسه کاربر، نام نقش و پیام خطا
    /// </summary>
    public InvalidUserRoleException(Guid userId, string roleName, string message)
        : this(message)
    {
        UserId = userId;
        RoleName = roleName;
    }

    /// <summary>
    /// ایجاد استثنا برای نقش ناموجود
    /// </summary>
    public static InvalidUserRoleException ForNonExistentRole(string roleName)
    {
        return new InvalidUserRoleException($"نقش '{roleName}' وجود ندارد");
    }

    /// <summary>
    /// ایجاد استثنا برای افزودن نقش تکراری
    /// </summary>
    public static InvalidUserRoleException ForDuplicateRole(string roleName)
    {
        return new InvalidUserRoleException($"کاربر قبلاً نقش '{roleName}' را دارد");
    }

    /// <summary>
    /// ایجاد استثنا برای حذف نقش پیش‌فرض
    /// </summary>
    public static InvalidUserRoleException ForRemovingDefaultRole(string roleName)
    {
        return new InvalidUserRoleException($"نقش '{roleName}' یک نقش پیش‌فرض است و نمی‌تواند حذف شود");
    }

    /// <summary>
    /// ایجاد استثنا برای تغییر به نقش نامعتبر
    /// </summary>
    public static InvalidUserRoleException ForInvalidRoleChange(string fromRole, string toRole)
    {
        return new InvalidUserRoleException($"تغییر از نقش '{fromRole}' به '{toRole}' مجاز نیست");
    }

    /// <summary>
    /// ایجاد استثنا برای کاربر فاقد هرگونه نقش
    /// </summary>
    public static InvalidUserRoleException ForNoRoles()
    {
        return new InvalidUserRoleException("کاربر باید حداقل یک نقش داشته باشد");
    }
}