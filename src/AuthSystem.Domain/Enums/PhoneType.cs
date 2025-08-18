// File: AuthSystem.Domain/Enums/PhoneType.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع شماره تلفن
/// - این enum برای تمایز بین شماره‌های موبایل و ثابت استفاده می‌شود
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
    /// شماره تلفن ویژه (مثلاً شماره‌های 909)
    /// </summary>
    Special = 3,

    /// <summary>
    /// شماره فکس
    /// </summary>
    Fax = 4
}