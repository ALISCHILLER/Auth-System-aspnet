using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Enums;


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
    /// سازنده با نام نقش و پیام خطا
    /// </summary>
    private InvalidUserRoleException(string roleName, string message)
        : this(message)
    {
        RoleName = roleName;
    }


    /// <summary>
    /// ایجاد استثنا برای نقش ناموجود
    /// </summary>
    public static InvalidUserRoleException ForNonExistentRole(string roleName)
    {
        return new InvalidUserRoleException(roleName, $"نقش '{roleName}' وجود ندارد");
    }

    /// <summary>
    /// ایجاد استثنا برای افزودن نقش تکراری
    /// </summary>
    public static InvalidUserRoleException ForDuplicateRole(string roleName)
    {
        return new InvalidUserRoleException(roleName, $"کاربر قبلاً نقش '{roleName}' را دارد");
    }
    /// ایجاد استثنا برای کاربری که در نقش مشخصی عضویت ندارد
    /// </summary>
    public static InvalidUserRoleException ForUserNotInRole(Guid userId, string roleName)
    {
        return new InvalidUserRoleException(userId, roleName, $"کاربر با شناسه {userId} در نقش '{roleName}' یافت نشد");
    }

    /// <summary>
    /// ایجاد استثنا برای زمانی که کاربر نقش مورد نظر را ندارد
    /// </summary>
    public static InvalidUserRoleException ForRoleAssignmentNotFound(Guid roleId)
    {
        return new InvalidUserRoleException($"کاربر نقش با شناسه '{roleId}' را ندارد");
    }

    /// <summary>
    /// ایجاد استثنا برای عدم یافتن مجوز در نقش مشخص
    /// </summary>
    public static InvalidUserRoleException ForPermissionNotFound(string roleName, PermissionType permission)
    {
        return new InvalidUserRoleException(roleName, $"مجوز '{permission}' برای نقش '{roleName}' یافت نشد");
    }

    /// <summary>
    /// ایجاد استثنا برای حذف نقش پیش‌فرض
    /// </summary>
    public static InvalidUserRoleException ForRemovingDefaultRole(string roleName)
    {
        return new InvalidUserRoleException(roleName, $"نقش '{roleName}' یک نقش پیش‌فرض است و نمی‌تواند حذف شود");
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