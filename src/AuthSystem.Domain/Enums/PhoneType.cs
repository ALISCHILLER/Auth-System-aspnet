namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع شماره تلفن
/// این enum برای تشخیص نوع شماره تلفن استفاده می‌شود
/// </summary>
public enum PhoneType
{
    /// <summary>
    /// شماره موبایل
    /// </summary>
    Mobile = 1,

    /// <summary>
    /// شماره تلفن ثابت
    /// </summary>
    Landline = 2,

    /// <summary>
    /// شماره فکس
    /// </summary>
    Fax = 3,

    /// <summary>
    /// شماره تول فری
    /// </summary>
    TollFree = 4,

    /// <summary>
    /// شماره اختصاصی
    /// </summary>
    PremiumRate = 5
}