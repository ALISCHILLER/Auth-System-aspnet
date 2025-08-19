using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// اکستنشن‌های وضعیت کاربر
/// </summary>
public static class UserStatusExtensions
{
    /// <summary>
    /// آیا کاربر فعال است
    /// </summary>
    public static bool IsActive(this UserStatus status)
    {
        return status == UserStatus.Active;
    }

    /// <summary>
    /// آیا کاربر قفل شده است
    /// </summary>
    public static bool IsLocked(this UserStatus status)
    {
        return status == UserStatus.Locked;
    }

    /// <summary>
    /// آیا کاربر در حالت انتظار است
    /// </summary>
    public static bool IsPending(this UserStatus status)
    {
        return status == UserStatus.Pending;
    }

    /// <summary>
    /// آیا کاربر حذف شده است
    /// </summary>
    public static bool IsDeleted(this UserStatus status)
    {
        return status == UserStatus.Deleted;
    }

    /// <summary>
    /// آیا کاربر تأیید نشده است
    /// </summary>
    public static bool IsUnverified(this UserStatus status)
    {
        return status == UserStatus.Unverified;
    }

    /// <summary>
    /// دریافت پیام وضعیت
    /// </summary>
    public static string GetStatusMessage(this UserStatus status)
    {
        return status switch
        {
            UserStatus.Active => "حساب کاربری فعال است",
            UserStatus.Locked => "حساب کاربری قفل شده است",
            UserStatus.Pending => "حساب کاربری در انتظار تأیید است",
            UserStatus.Deleted => "حساب کاربری حذف شده است",
            UserStatus.Unverified => "حساب کاربری تأیید نشده است",
            _ => "وضعیت نامشخص"
        };
    }

    /// <summary>
    /// دریافت رنگ نمایش وضعیت
    /// </summary>
    public static string GetStatusColor(this UserStatus status)
    {
        return status switch
        {
            UserStatus.Active => "success",
            UserStatus.Locked => "danger",
            UserStatus.Pending => "warning",
            UserStatus.Deleted => "secondary",
            UserStatus.Unverified => "info",
            _ => "dark"
        };
    }
}