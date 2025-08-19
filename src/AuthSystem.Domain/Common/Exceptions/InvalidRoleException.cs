using System;
using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای موارد نامعتبر بودن نقش
/// </summary>
public class InvalidRoleException : DomainException
{
    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidRole";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidRoleException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidRoleException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// استثنا برای مواردی که نام نقش معتبر نیست
/// </summary>
public class InvalidRoleNameException : InvalidRoleException
{
    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidRoleName";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidRoleNameException(string message) : base(message)
    {
    }
}

/// <summary>
/// استثنا برای مواردی که توضیحات نقش معتبر نیست
/// </summary>
public class InvalidRoleDescriptionException : InvalidRoleException
{
    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidRoleDescription";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidRoleDescriptionException(string message) : base(message)
    {
    }
}

/// <summary>
/// استثنا برای مواردی که مجوز نمی‌تواند حذف شود
/// </summary>
public class RolePermissionCannotBeRemovedException : InvalidRoleException
{
    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "RolePermissionCannotBeRemoved";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public RolePermissionCannotBeRemovedException(string message) : base(message)
    {
    }
}