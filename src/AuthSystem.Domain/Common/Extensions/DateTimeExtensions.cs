using System;
using AuthSystem.Domain.Common.Clock;


namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// DateTime helper extensions tailored for the domain.
/// </summary>
public static class DateTimeExtensions
{
  
    public static DateTime ToIranTime(this DateTime dateTime)
    {
        var iranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime(), iranTimeZone);
    }

    public static DateTime ToUtcTime(this DateTime dateTime)
    {
        return dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Unspecified => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc),
            _ => dateTime.ToUniversalTime()
        };
    }

    public static bool IsFuture(this DateTime dateTime) => dateTime > DomainClock.Instance.UtcNow;

    public static bool IsPast(this DateTime dateTime) => dateTime < DomainClock.Instance.UtcNow;

    public static string ToPersianDate(this DateTime dateTime) => $"{dateTime.Year}/{dateTime.Month:D2}/{dateTime.Day:D2}";


    public static int CalculateAge(this DateTime birthDate)
    {
        var today = DomainClock.Instance.UtcNow.Date;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }

    public static DateTime RoundToMinute(this DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, DateTimeKind.Utc);

    public static DateTime RoundToHour(this DateTime dateTime) =>
         new(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, DateTimeKind.Utc);

    public static DateTime RoundToDay(this DateTime dateTime) =>
       new(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
}