using AuthSystem.Domain.Common.Clock;
using System.Globalization;


namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// UTC-first + cross-platform Tehran TZ + PersianCalendar formatting.
/// </summary>
public static class DateTimeExtensions
{

    // Windows: "Iran Standard Time", Linux/macOS: "Asia/Tehran"
    private static readonly string[] IranTimeZoneIds = new[] { "Asia/Tehran", "Iran Standard Time" };

    /// <summary>
    /// Converts the provided date to Iran local time (Tehran) safely across platforms.
    /// </summary>
    public static DateTime ToIranTime(this DateTime dateTime)
    {
        var utc = EnsureUtc(dateTime);
        var tz = GetIranTimeZone();
        return TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
    }

    /// <summary>
    /// Normalizes the provided date to UTC preserving precision.
    /// </summary>
    public static DateTime ToUtcTime(this DateTime dateTime) => EnsureUtc(dateTime);

    public static bool IsFuture(this DateTime dateTime) => EnsureUtc(dateTime) > DomainClock.Instance.UtcNow;

    public static bool IsPast(this DateTime dateTime) => EnsureUtc(dateTime) < DomainClock.Instance.UtcNow;

    /// <summary>
    /// Formats the date in Persian (Jalaali) using Tehran local time.
    /// </summary>
    public static string ToPersianDate(this DateTime dateTime)
    {
        var tehran = dateTime.ToIranTime();
        var pc = new PersianCalendar();
        var y = pc.GetYear(tehran);
        var m = pc.GetMonth(tehran);
        var d = pc.GetDayOfMonth(tehran);
        return $"{y}/{m:D2}/{d:D2}";
    }


    /// <summary>
    /// Calculates age in full years based on UTC "today".
    /// </summary>
    public static int CalculateAge(this DateTime birthDate)
    {
        var birth = EnsureUtc(birthDate).Date;
        var today = DomainClock.Instance.UtcNow.Date;
        var age = today.Year - birth.Year;
        if (birth > today.AddYears(-age)) age--;
        return age;
    }

    public static DateTime RoundToMinute(this DateTime dateTime)
    {
        var utc = EnsureUtc(dateTime);
        return new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, 0, DateTimeKind.Utc);
    }

    public static DateTime RoundToHour(this DateTime dateTime)
    {
        var utc = EnsureUtc(dateTime);
        return new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, 0, 0, DateTimeKind.Utc);
    }

    public static DateTime RoundToDay(this DateTime dateTime)
    {
        var utc = EnsureUtc(dateTime);
        return new DateTime(utc.Year, utc.Month, utc.Day, 0, 0, 0, DateTimeKind.Utc);
    }

    private static DateTime EnsureUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
        _ => value.ToUniversalTime()
    };

    private static TimeZoneInfo GetIranTimeZone()
    {
        foreach (var id in IranTimeZoneIds)
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
            catch (TimeZoneNotFoundException) { /* try next */ }
            catch (InvalidTimeZoneException) { /* try next */ }
        }

        var offset = TimeSpan.FromHours(3.5);
        return TimeZoneInfo.CreateCustomTimeZone("Asia/Tehran", offset, "Iran Standard Time", "Iran Standard Time");
    }
}