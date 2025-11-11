using AuthSystem.Domain.Common.Base;
using AuthSystem.Domain.Enums;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object برای User Agent
/// تحلیل و استخراج اطلاعات از User Agent string
/// </summary>
public sealed class UserAgent : ValueObject
{
    /// <summary>
    /// حداکثر طول مجاز
    /// </summary>
    private const int MaxLength = 500;

    /// <summary>
    /// الگوهای تشخیص مرورگر
    /// </summary>
    private static readonly Dictionary<string, Regex> BrowserPatterns = new()
    {
        { "Chrome", new Regex(@"Chrome/(\d+\.\d+)", RegexOptions.IgnoreCase) },
        { "Firefox", new Regex(@"Firefox/(\d+\.\d+)", RegexOptions.IgnoreCase) },
        { "Safari", new Regex(@"Version/(\d+\.\d+).*Safari", RegexOptions.IgnoreCase) },
        { "Edge", new Regex(@"Edg/(\d+\.\d+)", RegexOptions.IgnoreCase) },
        { "Opera", new Regex(@"OPR/(\d+\.\d+)", RegexOptions.IgnoreCase) }
    };

    /// <summary>
    /// الگوهای تشخیص سیستم عامل
    /// </summary>
    private static readonly Dictionary<string, Regex> OsPatterns = new()
    {
        { "Windows", new Regex(@"Windows NT (\d+\.\d+)", RegexOptions.IgnoreCase) },
        { "macOS", new Regex(@"Mac OS X (\d+[._]\d+)", RegexOptions.IgnoreCase) },
        { "Linux", new Regex(@"Linux", RegexOptions.IgnoreCase) },
        { "Android", new Regex(@"Android (\d+\.\d+)", RegexOptions.IgnoreCase) },
        { "iOS", new Regex(@"OS (\d+[._]\d+) like Mac OS X", RegexOptions.IgnoreCase) }
    };

    /// <summary>
    /// مقدار User Agent
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// نام مرورگر
    /// </summary>
    public string Browser { get; }

    /// <summary>
    /// نسخه مرورگر
    /// </summary>
    public string? BrowserVersion { get; }

    /// <summary>
    /// سیستم عامل
    /// </summary>
    public string OperatingSystem { get; }

    /// <summary>
    /// نسخه سیستم عامل
    /// </summary>
    public string? OsVersion { get; }

    /// <summary>
    /// آیا موبایل است
    /// </summary>
    public bool IsMobile { get; }

    /// <summary>
    /// آیا ربات است
    /// </summary>
    public bool IsBot { get; }

    /// <summary>
    /// نوع دستگاه
    /// </summary>
    public DeviceType DeviceType { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private UserAgent(string value)
    {
        Value = value;
        Browser = DetectBrowser(value, out var browserVersion);
        BrowserVersion = browserVersion;
        OperatingSystem = DetectOs(value, out var osVersion);
        OsVersion = osVersion;
        IsMobile = DetectMobile(value);
        IsBot = DetectBot(value);
        DeviceType = DetermineDeviceType(value);
    }

    /// <summary>
    /// ایجاد UserAgent معتبر
    /// </summary>
    public static UserAgent Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("User Agent نمی‌تواند خالی باشد");

        if (value.Length > MaxLength)
            throw new ArgumentException($"User Agent نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

        return new UserAgent(value);
    }

    /// <summary>
    /// ایجاد UserAgent ناشناخته
    /// </summary>
    public static UserAgent Unknown => new("Unknown");

    /// <summary>
    /// تشخیص مرورگر
    /// </summary>
    private static string DetectBrowser(string userAgent, out string? version)
    {
        version = null;

        foreach (var pattern in BrowserPatterns)
        {
            var match = pattern.Value.Match(userAgent);
            if (match.Success)
            {
                version = match.Groups[1].Value;
                return pattern.Key;
            }
        }

        return "Unknown";
    }

    /// <summary>
    /// تشخیص سیستم عامل
    /// </summary>
    private static string DetectOs(string userAgent, out string? version)
    {
        version = null;

        foreach (var pattern in OsPatterns)
        {
            var match = pattern.Value.Match(userAgent);
            if (match.Success)
            {
                if (match.Groups.Count > 1)
                    version = match.Groups[1].Value.Replace('_', '.');
                return pattern.Key;
            }
        }

        return "Unknown";
    }

    /// <summary>
    /// تشخیص موبایل
    /// </summary>
    private static bool DetectMobile(string userAgent)
    {
        var mobileKeywords = new[]
        {
            "Mobile", "Android", "iPhone", "iPad", "Windows Phone",
            "BlackBerry", "Opera Mini", "IEMobile"
        };

        return Array.Exists(mobileKeywords,
            keyword => userAgent.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// تشخیص ربات
    /// </summary>
    private static bool DetectBot(string userAgent)
    {
        var botKeywords = new[]
        {
            "bot", "crawler", "spider", "scraper", "wget", "curl",
            "python", "java", "ruby", "go-http-client"
        };

        return Array.Exists(botKeywords,
            keyword => userAgent.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// تعیین نوع دستگاه
    /// </summary>
    private static DeviceType DetermineDeviceType(string userAgent)
    {
        if (userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase))
            return DeviceType.Tablet;

        if (DetectMobile(userAgent))
            return DeviceType.Mobile;

        if (DetectBot(userAgent))
            return DeviceType.Bot;

        return DeviceType.Desktop;
    }

    /// <summary>
    /// دریافت نام کامل مرورگر با نسخه
    /// </summary>
    public string GetBrowserFullName()
    {
        return BrowserVersion != null
            ? $"{Browser} {BrowserVersion}"
            : Browser;
    }

    /// <summary>
    /// دریافت نام کامل سیستم عامل با نسخه
    /// </summary>
    public string GetOsFullName()
    {
        return OsVersion != null
            ? $"{OperatingSystem} {OsVersion}"
            : OperatingSystem;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}