using AuthSystem.Domain.Enums;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// اکستنشن‌های کد تایید
/// </summary>
public static class VerificationCodeExtensions
{
    /// <summary>
    /// آیا کد تایید معتبر است
    /// </summary>
    public static bool IsValid(this VerificationCode code)
    {
        return code.IsValid;
    }

    /// <summary>
    /// آیا کد تایید منقضی شده است
    /// </summary>
    public static bool IsExpired(this VerificationCode code)
    {
        return code.IsExpired;
    }

    /// <summary>
    /// آیا کد تایید استفاده شده است
    /// </summary>
    public static bool IsUsed(this VerificationCode code)
    {
        return code.IsUsed;
    }

    /// <summary>
    /// دریافت تعداد تلاش‌های باقی‌مانده
    /// </summary>
    public static int RemainingAttempts(this VerificationCode code)
    {
        return code.RemainingAttempts;
    }

    /// <summary>
    /// دریافت زمان باقی‌مانده تا انقضا
    /// </summary>
    public static TimeSpan? TimeToExpiry(this VerificationCode code)
    {
        return code.TimeToExpiry;
    }

    /// <summary>
    /// دریافت کد با فرمت مناسب برای نمایش
    /// </summary>
    public static string GetFormattedCode(this VerificationCode code)
    {
        return code.GetFormattedCode();
    }

    /// <summary>
    /// تولید کد تایید عددی جدید
    /// </summary>
    public static VerificationCode GenerateNumericCode(
        VerificationCodeType type,
        int length = 6,
        int? validityMinutes = null)
    {
        return VerificationCode.GenerateNumeric(type, length, validityMinutes);
    }

    /// <summary>
    /// تولید کد تایید الفبایی-عددی جدید
    /// </summary>
    public static VerificationCode GenerateAlphanumericCode(
        VerificationCodeType type,
        int length = 6,
        int? validityMinutes = null)
    {
        return VerificationCode.GenerateAlphanumeric(type, length, validityMinutes);
    }

    /// <summary>
    /// تولید کد تایید بر اساس نوع
    /// </summary>
    public static VerificationCode GenerateCode(
        VerificationCodeType type,
        CodeFormat format = CodeFormat.Numeric,
        int length = 6,
        int? validityMinutes = null)
    {
        return format switch
        {
            CodeFormat.Numeric => VerificationCode.GenerateNumeric(type, length, validityMinutes),
            CodeFormat.Alphanumeric => VerificationCode.GenerateAlphanumeric(type, length, validityMinutes),
            _ => throw new ArgumentException("Invalid code format")
        };
    }

    /// <summary>
    /// ثبت تلاش ناموفق
    /// </summary>
    public static VerificationCode RecordFailedAttempt(this VerificationCode code)
    {
        return code.RecordFailedAttempt();
    }

    /// <summary>
    /// علامت‌گذاری به عنوان استفاده شده
    /// </summary>
    public static VerificationCode MarkAsUsed(this VerificationCode code)
    {
        return code.MarkAsUsed();
    }
}