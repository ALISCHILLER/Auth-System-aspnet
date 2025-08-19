using System;

namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// اکستنشن‌های تاریخ و زمان
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// تبدیل به زمان محلی ایران
    /// </summary>
    public static DateTime ToIranTime(this DateTime dateTime)
    {
        var iranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, iranTimeZone);
    }

    /// <summary>
    /// تبدیل به زمان UTC
    /// </summary>
    public static DateTime ToUtcTime(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc)
            return dateTime;

        return dateTime.Kind == DateTimeKind.Unspecified
            ? new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Utc)
            : dateTime.ToUniversalTime();
    }

    /// <summary>
    /// آیا تاریخ در آینده است
    /// </summary>
    public static bool IsFuture(this DateTime dateTime)
    {
        return dateTime > DateTime.UtcNow;
    }

    /// <summary>
    /// آیا تاریخ در گذشته است
    /// </summary>
    public static bool IsPast(this DateTime dateTime)
    {
        return dateTime < DateTime.UtcNow;
    }

    /// <summary>
    /// تبدیل به فرمت تاریخ شمسی
    /// </summary>
    public static string ToPersianDate(this DateTime dateTime)
    {
        // در عمل از کتابخانه‌های تبدیل تاریخ استفاده کنید
        // این فقط یک نمونه است
        return $"{dateTime.Year}/{dateTime.Month}/{dateTime.Day}";
    }

    /// <summary>
    /// محاسبه سن از تاریخ تولد
    /// </summary>
    public static int CalculateAge(this DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;
        return age;
    }

    /// <summary>
    /// گرد کردن تاریخ به دقیقه
    /// </summary>
    public static DateTime RoundToMinute(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
    }

    /// <summary>
    /// گرد کردن تاریخ به ساعت
    /// </summary>
    public static DateTime RoundToHour(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
    }

    /// <summary>
    /// گرد کردن تاریخ به روز
    /// </summary>
    public static DateTime RoundToDay(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
    }
}