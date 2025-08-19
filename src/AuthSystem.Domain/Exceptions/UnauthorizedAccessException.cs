using AuthSystem.Domain.Common.Exceptions;
using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای دسترسی غیرمجاز
/// این استثنا زمانی رخ می‌دهد که کاربر سعی کند به منابعی دسترسی پیدا کند که مجوز لازم را ندارد
/// </summary>
public class UnauthorizedAccessException : DomainException
{
    /// <summary>
    /// منبع درخواستی
    /// </summary>
    public string Resource { get; }

    /// <summary>
    /// مجوز مورد نیاز
    /// </summary>
    public string RequiredPermission { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "UnauthorizedAccess";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public UnauthorizedAccessException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public UnauthorizedAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با منبع و مجوز مورد نیاز
    /// </summary>
    public UnauthorizedAccessException(string resource, string requiredPermission)
        : base($"دسترسی به '{resource}' نیاز به مجوز '{requiredPermission}' دارد")
    {
        Resource = resource;
        RequiredPermission = requiredPermission;
    }

    /// <summary>
    /// ایجاد استثنا برای دسترسی غیرمجاز به API
    /// </summary>
    public static UnauthorizedAccessException ForApiAccess(string apiEndpoint)
    {
        return new UnauthorizedAccessException(apiEndpoint, "ApiAccess");
    }

    /// <summary>
    /// ایجاد استثنا برای دسترسی غیرمجاز به منبع
    /// </summary>
    public static UnauthorizedAccessException ForResourceAccess(string resource, string permission)
    {
        return new UnauthorizedAccessException(resource, permission);
    }

    /// <summary>
    /// ایجاد استثنا برای دسترسی غیرمجاز به عملیات مدیریتی
    /// </summary>
    public static UnauthorizedAccessException ForAdminOperation()
    {
        return new UnauthorizedAccessException("AdminOperation", "Admin");
    }

    /// <summary>
    /// ایجاد استثنا برای دسترسی غیرمجاز به اطلاعات کاربر دیگر
    /// </summary>
    public static UnauthorizedAccessException ForAccessingOtherUserAccount(Guid targetUserId)
    {
        return new UnauthorizedAccessException($"UserAccount/{targetUserId}", "ViewOtherUsers");
    }

    /// <summary>
    /// ایجاد استثنا برای دسترسی غیرمجاز به عملیات حساس
    /// </summary>
    public static UnauthorizedAccessException ForSensitiveOperation(string operation)
    {
        return new UnauthorizedAccessException(operation, "SensitiveOperation");
    }
}