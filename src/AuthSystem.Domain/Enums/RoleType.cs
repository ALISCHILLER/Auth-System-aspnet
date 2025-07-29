namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع نقش‌های سیستم
/// این enum برای تمایز بین نقش‌های سیستمی و نقش‌های کاربری استفاده می‌شود
/// </summary>
public enum RoleType
{
    /// <summary>
    /// نقش سیستمی (مانند Admin, User)
    /// این نقش‌ها توسط سیستم تعریف شده‌اند و نمی‌توان آن‌ها را حذف کرد
    /// </summary>
    System,

    /// <summary>
    /// نقش کاربری (مانند Manager, Editor)
    /// این نقش‌ها توسط ادمین سیستم تعریف شده‌اند و می‌توان آن‌ها را مدیریت کرد
    /// </summary>
    Custom
}