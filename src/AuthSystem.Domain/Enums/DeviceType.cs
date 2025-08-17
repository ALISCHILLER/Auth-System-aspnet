using System.ComponentModel;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع دستگاه‌هایی که کاربران از آنها برای دسترسی به سیستم استفاده می‌کنند
/// برای مدیریت جلسات و امنیت بر اساس نوع دستگاه
/// </summary>
public enum DeviceType
{
    /// <summary>
    /// نوع دستگاه نامشخص است
    /// </summary>
    [Description("نامشخص")]
    Unknown = 0,

    /// <summary>
    /// مرورگر وب دسکتاپ
    /// شامل Chrome، Firefox، Safari، Edge و غیره
    /// </summary>
    [Description("مرورگر دسکتاپ")]
    DesktopBrowser = 1,

    /// <summary>
    /// مرورگر موبایل
    /// شامل Chrome Mobile، Safari Mobile و غیره
    /// </summary>
    [Description("مرورگر موبایل")]
    MobileBrowser = 2,

    /// <summary>
    /// اپلیکیشن موبایل اندروید
    /// </summary>
    [Description("اندروید")]
    AndroidApp = 3,

    /// <summary>
    /// اپلیکیشن موبایل iOS
    /// </summary>
    [Description("آی‌او‌اس")]
    iOSApp = 4,

    /// <summary>
    /// اپلیکیشن ویندوز
    /// </summary>
    [Description("ویندوز")]
    WindowsApp = 5,

    /// <summary>
    /// اپلیکیشن macOS
    /// </summary>
    [Description("مک")]
    MacApp = 6,

    /// <summary>
    /// اپلیکیشن لینوکس
    /// </summary>
    [Description("لینوکس")]
    LinuxApp = 7,

    /// <summary>
    /// تبلت (مرورگر یا اپلیکیشن)
    /// </summary>
    [Description("تبلت")]
    Tablet = 8,

    /// <summary>
    /// تلویزیون هوشمند
    /// </summary>
    [Description("تلویزیون هوشمند")]
    SmartTV = 9,

    /// <summary>
    /// کنسول بازی
    /// شامل PlayStation، Xbox و غیره
    /// </summary>
    [Description("کنسول بازی")]
    GameConsole = 10,

    /// <summary>
    /// دستگاه IoT
    /// اینترنت اشیا
    /// </summary>
    [Description("اینترنت اشیا")]
    IoTDevice = 11,

    /// <summary>
    /// ساعت هوشمند
    /// </summary>
    [Description("ساعت هوشمند")]
    SmartWatch = 12,

    /// <summary>
    /// ربات یا اسکریپت
    /// برای دسترسی‌های API
    /// </summary>
    [Description("ربات")]
    Bot = 13,

    /// <summary>
    /// CLI یا Terminal
    /// دسترسی از طریق خط فرمان
    /// </summary>
    [Description("خط فرمان")]
    CLI = 14,

    /// <summary>
    /// سرویس یا API
    /// برای ارتباطات سرور به سرور
    /// </summary>
    [Description("سرویس")]
    Service = 15,

    /// <summary>
    /// مرورگر ناشناس یا حالت خصوصی
    /// </summary>
    [Description("مرورگر ناشناس")]
    IncognitoBrowser = 16,

    /// <summary>
    /// اپلیکیشن Progressive Web App
    /// </summary>
    [Description("PWA")]
    PWA = 17,

    /// <summary>
    /// کیوسک یا دستگاه عمومی
    /// </summary>
    [Description("کیوسک")]
    Kiosk = 18,

    /// <summary>
    /// رایانه مجازی یا VDI
    /// </summary>
    [Description("رایانه مجازی")]
    VirtualDesktop = 19
}

/// <summary>
/// متدهای کمکی برای DeviceType
/// </summary>
public static class DeviceTypeExtensions
{
    /// <summary>
    /// بررسی موبایل بودن دستگاه
    /// </summary>
    public static bool IsMobile(this DeviceType deviceType)
    {
        return deviceType == DeviceType.MobileBrowser ||
               deviceType == DeviceType.AndroidApp ||
               deviceType == DeviceType.iOSApp ||
               deviceType == DeviceType.SmartWatch;
    }

    /// <summary>
    /// بررسی دسکتاپ بودن دستگاه
    /// </summary>
    public static bool IsDesktop(this DeviceType deviceType)
    {
        return deviceType == DeviceType.DesktopBrowser ||
               deviceType == DeviceType.WindowsApp ||
               deviceType == DeviceType.MacApp ||
               deviceType == DeviceType.LinuxApp ||
               deviceType == DeviceType.VirtualDesktop;
    }

    /// <summary>
    /// بررسی مرورگر بودن
    /// </summary>
    public static bool IsBrowser(this DeviceType deviceType)
    {
        return deviceType == DeviceType.DesktopBrowser ||
               deviceType == DeviceType.MobileBrowser ||
               deviceType == DeviceType.IncognitoBrowser;
    }

    /// <summary>
    /// بررسی اپلیکیشن بودن
    /// </summary>
    public static bool IsApp(this DeviceType deviceType)
    {
        return deviceType == DeviceType.AndroidApp ||
               deviceType == DeviceType.iOSApp ||
               deviceType == DeviceType.WindowsApp ||
               deviceType == DeviceType.MacApp ||
               deviceType == DeviceType.LinuxApp ||
               deviceType == DeviceType.PWA;
    }

    /// <summary>
    /// بررسی قابل اعتماد بودن دستگاه
    /// </summary>
    public static bool IsTrusted(this DeviceType deviceType)
    {
        return deviceType != DeviceType.Unknown &&
               deviceType != DeviceType.Bot &&
               deviceType != DeviceType.IncognitoBrowser &&
               deviceType != DeviceType.Kiosk;
    }

    /// <summary>
    /// بررسی نیاز به محدودیت‌های اضافی
    /// </summary>
    public static bool RequiresAdditionalSecurity(this DeviceType deviceType)
    {
        return deviceType == DeviceType.Unknown ||
               deviceType == DeviceType.Bot ||
               deviceType == DeviceType.Service ||
               deviceType == DeviceType.Kiosk ||
               deviceType == DeviceType.IncognitoBrowser;
    }

    /// <summary>
    /// دریافت مدت زمان جلسه پیشنهادی (دقیقه)
    /// </summary>
    public static int GetSuggestedSessionDuration(this DeviceType deviceType)
    {
        return deviceType switch
        {
            DeviceType.DesktopBrowser => 1440, // 24 ساعت
            DeviceType.MobileBrowser => 720,   // 12 ساعت
            DeviceType.AndroidApp => 10080,    // 7 روز
            DeviceType.iOSApp => 10080,        // 7 روز
            DeviceType.WindowsApp => 20160,    // 14 روز
            DeviceType.MacApp => 20160,        // 14 روز
            DeviceType.LinuxApp => 20160,      // 14 روز
            DeviceType.Tablet => 2880,         // 2 روز
            DeviceType.SmartTV => 43200,       // 30 روز
            DeviceType.SmartWatch => 1440,     // 24 ساعت
            DeviceType.Bot => 60,              // 1 ساعت
            DeviceType.Service => 60,          // 1 ساعت
            DeviceType.CLI => 480,             // 8 ساعت
            DeviceType.Kiosk => 30,            // 30 دقیقه
            DeviceType.IncognitoBrowser => 60, // 1 ساعت
            DeviceType.PWA => 10080,           // 7 روز
            DeviceType.VirtualDesktop => 480,   // 8 ساعت
            _ => 60                            // 1 ساعت پیش‌فرض
        };
    }

    /// <summary>
    /// بررسی اجازه چند جلسه همزمان
    /// </summary>
    public static bool AllowsMultipleSessions(this DeviceType deviceType)
    {
        return deviceType != DeviceType.Bot &&
               deviceType != DeviceType.Service &&
               deviceType != DeviceType.Kiosk;
    }

    /// <summary>
    /// دریافت حداکثر تعداد جلسات همزمان
    /// </summary>
    public static int GetMaxConcurrentSessions(this DeviceType deviceType)
    {
        return deviceType switch
        {
            DeviceType.Bot => 1,
            DeviceType.Service => 1,
            DeviceType.Kiosk => 1,
            DeviceType.SmartTV => 1,
            DeviceType.GameConsole => 1,
            DeviceType.SmartWatch => 1,
            DeviceType.MobileBrowser => 2,
            DeviceType.AndroidApp => 2,
            DeviceType.iOSApp => 2,
            DeviceType.Tablet => 2,
            _ => 3
        };
    }
}
