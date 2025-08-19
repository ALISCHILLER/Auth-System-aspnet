using System;

namespace AuthSystem.Domain.Services.Contracts;

/// <summary>
/// اینترفیس برای محدود کردن نرخ درخواست‌ها
/// این اینترفیس قراردادهای لازم برای جلوگیری از حملات DDoS و Brute Force را تعریف می‌کند
/// </summary>
public interface IRateLimiter
{
    /// <summary>
    /// بررسی آیا درخواست مجاز است
    /// </summary>
    /// <param name="key">کلید برای شناسایی درخواست (معمولاً آدرس IP یا شناسه کاربر)</param>
    /// <returns>نتیجه بررسی محدودیت نرخ</returns>
    RateLimitResult CheckLimit(string key);

    /// <summary>
    /// بررسی آیا درخواست مجاز است با تنظیمات سفارشی
    /// </summary>
    /// <param name="key">کلید برای شناسایی درخواست</param>
    /// <param name="maxRequests">حداکثر تعداد درخواست</param>
    /// <param name="window">پنجره زمانی (به ثانیه)</param>
    /// <returns>نتیجه بررسی محدودیت نرخ</returns>
    RateLimitResult CheckLimit(string key, int maxRequests, int window);

    /// <summary>
    /// ریست کردن محدودیت نرخ برای یک کلید
    /// </summary>
    /// <param name="key">کلید برای ریست</param>
    void ResetLimit(string key);

    /// <summary>
    /// ریست کردن تمام محدودیت‌های نرخ
    /// </summary>
    void ResetAllLimits();

    /// <summary>
    /// دریافت وضعیت فعلی محدودیت نرخ برای یک کلید
    /// </summary>
    /// <param name="key">کلید برای بررسی</param>
    /// <returns>وضعیت فعلی محدودیت نرخ</returns>
    RateLimitStatus GetStatus(string key);

    /// <summary>
    /// آیا سرویس محدودکننده نرخ فعال است
    /// </summary>
    /// <returns>وضعیت فعال بودن سرویس</returns>
    bool IsEnabled();
}

/// <summary>
/// نتیجه بررسی محدودیت نرخ
/// </summary>
public class RateLimitResult
{
    /// <summary>
    /// آیا درخواست مجاز است
    /// </summary>
    public bool IsAllowed { get; set; }

    /// <summary>
    /// تعداد درخواست‌های باقی‌مانده
    /// </summary>
    public int Remaining { get; set; }

    /// <summary>
    /// زمان بازنشانی محدودیت
    /// </summary>
    public DateTime Reset { get; set; }

    /// <summary>
    /// تعداد کل درخواست‌ها در این پنجره زمانی
    /// </summary>
    public int Total { get; set; }
}

/// <summary>
/// وضعیت فعلی محدودیت نرخ
/// </summary>
public class RateLimitStatus
{
    /// <summary>
    /// تعداد درخواست‌های انجام شده
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// زمان آخرین درخواست
    /// </summary>
    public DateTime LastRequest { get; set; }

    /// <summary>
    /// آیا در حال حاضر محدود شده است
    /// </summary>
    public bool IsLimited { get; set; }

    /// <summary>
    /// زمان بازنشانی محدودیت
    /// </summary>
    public DateTime ResetTime { get; set; }
}