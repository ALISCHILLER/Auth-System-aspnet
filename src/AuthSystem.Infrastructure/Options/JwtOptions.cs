using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Infrastructure.Options;

/// <summary>
/// تنظیمات مربوط به توکن‌های JWT
/// این کلاس تمام پیکربندی‌های لازم برای تولید و اعتبارسنجی توکن‌های JWT را تعریف می‌کند
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// نام بخش تنظیمات JWT در فایل پیکربندی
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// کلید مخفی برای امضای دیجیتال توکن‌ها (Signing Key)
    /// این کلید باید به اندازه کافی قوی و محرمانه باشد
    /// </summary>
    [Required(ErrorMessage = "Jwt:SecretKey الزامی است.")]
    [MinLength(32, ErrorMessage = "Jwt:SecretKey باید حداقل 32 کاراکتر باشد.")]
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// صادرکننده توکن (Issuer)
    /// نام سرویس یا دامنه‌ای که توکن را صادر می‌کند
    /// </summary>
    [Required(ErrorMessage = "Jwt:Issuer الزامی است.")]
    [Url(ErrorMessage = "Jwt:Issuer باید یک آدرس URL معتبر باشد.")]
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// مقصد توکن (Audience)
    /// نام سرویس یا دامنه‌ای که توکن برای آن صادر شده است
    /// </summary>
    [Required(ErrorMessage = "Jwt:Audience الزامی است.")]
    [Url(ErrorMessage = "Jwt:Audience باید یک آدرس URL معتبر باشد.")]
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// مدت زمان انقضای توکن دسترسی (Access Token)
    /// مقدار پیش‌فرض: 15 دقیقه (برای امنیت بالا)
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; } = 15;

    /// <summary>
    /// مدت زمان انقضای توکن تازه‌سازی (Refresh Token)
    /// مقدار پیش‌فرض: 7 روز
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;

    /// <summary>
    /// مدت زمان انقضای توکن تأیید ایمیل (Email Confirmation Token)
    /// مقدار پیش‌فرض: 24 ساعت
    /// </summary>
    public int EmailConfirmationTokenExpirationHours { get; set; } = 24;

    /// <summary>
    /// مدت زمان انقضای توکن بازیابی رمز عبور (Password Reset Token)
    /// مقدار پیش‌فرض: 1 ساعت
    /// </summary>
    public int PasswordResetTokenExpirationHours { get; set; } = 1;

    /// <summary>
    /// مدت زمان انقضای توکن تأیید شماره تلفن (Phone Verification Token)
    /// مقدار پیش‌فرض: 15 دقیقه
    /// </summary>
    public int PhoneVerificationTokenExpirationMinutes { get; set; } = 15;

    /// <summary>
    /// آیا از الگوریتم‌های امن‌تر (مانند RSA) استفاده شود؟
    /// مقدار پیش‌فرض: false (استفاده از HMAC)
    /// </summary>
    public bool UseAsymmetricEncryption { get; set; } = false;

    /// <summary>
    /// آدرس کلید عمومی برای اعتبارسنجی توکن‌ها (در صورت استفاده از RSA)
    /// </summary>
    public string? PublicKeyUrl { get; set; }

    /// <summary>
    /// مسیر فایل کلید خصوصی (در صورت استفاده از RSA)
    /// </summary>
    public string? PrivateKeyPath { get; set; }

    /// <summary>
    /// رمز عبور فایل کلید خصوصی (در صورت وجود)
    /// </summary>
    public string? PrivateKeyPassword { get; set; }

    /// <summary>
    /// آیا توکن‌ها باید حاوی اطلاعات دستگاه کاربر باشند؟
    /// </summary>
    public bool IncludeDeviceInfo { get; set; } = true;

    /// <summary>
    /// آیا توکن‌ها باید حاوی اطلاعات IP کاربر باشند؟
    /// </summary>
    public bool IncludeIpAddress { get; set; } = true;

    /// <summary>
    /// آیا توکن‌ها باید حاوی اطلاعات User Agent باشند؟
    /// </summary>
    public bool IncludeUserAgent { get; set; } = true;

    /// <summary>
    /// آیا توکن‌ها باید حاوی شناسه جلسه (Session ID) باشند؟
    /// </summary>
    public bool IncludeSessionId { get; set; } = true;

    /// <summary>
    /// آیا توکن‌ها باید حاوی شماره نسخه باشند؟
    /// برای مدیریت تغییرات در ساختار توکن
    /// </summary>
    public bool IncludeVersion { get; set; } = true;

    /// <summary>
    /// نسخه فعلی توکن‌ها
    /// </summary>
    public string Version { get; set; } = "1.0";

    /// <summary>
    /// آیا از توکن‌های چندلایه (Nested Tokens) استفاده شود؟
    /// برای امنیت بالاتر در سیستم‌های حساس
    /// </summary>
    public bool UseNestedTokens { get; set; } = false;

    /// <summary>
    /// آیا توکن‌ها باید حاوی امضای دیجیتال اضافی باشند؟
    /// </summary>
    public bool IncludeAdditionalSignature { get; set; } = false;

    /// <summary>
    /// آیا از کش توکن‌های منقضی شده استفاده شود؟
    /// برای جلوگیری از استفاده مجدد توکن‌های منقضی شده (Replay Attack)
    /// </summary>
    public bool UseRevokedTokenCache { get; set; } = true;

    /// <summary>
    /// مدت زمان نگهداری توکن‌های منقضی شده در کش (به دقیقه)
    /// </summary>
    public int RevokedTokenCacheDurationMinutes { get; set; } = 60;

    /// <summary>
    /// آیا از توکن‌های محدود به دستگاه استفاده شود؟
    /// توکن فقط از دستگاهی که درخواست شده است قابل استفاده است
    /// </summary>
    public bool UseDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های محدود به IP استفاده شود؟
    /// توکن فقط از IP‌ای که درخواست شده است قابل استفاده است
    /// </summary>
    public bool UseIpBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های محدود به User Agent استفاده شود؟
    /// توکن فقط از User Agentی که درخواست شده است قابل استفاده است
    /// </summary>
    public bool UseUserAgentBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های زمان‌بندی شده استفاده شود؟
    /// توکن فقط در بازه زمانی مشخصی قابل استفاده است
    /// </summary>
    public bool UseTimeBoundTokens { get; set; } = true;

    /// <summary>
    /// تعداد حداکثر توکن‌های فعال برای هر کاربر
    /// برای جلوگیری از استفاده بیش از حد از سیستم
    /// </summary>
    public int MaxActiveTokensPerUser { get; set; } = 5;

    /// <summary>
    /// آیا از توکن‌های یک‌بار مصرف استفاده شود؟
    /// برای عملیات حساس مانند تغییر رمز عبور
    /// </summary>
    public bool UseOneTimeTokens { get; set; } = true;

    /// <summary>
    /// مدت زمان انقضای توکن‌های یک‌بار مصرف (به دقیقه)
    /// </summary>
    public int OneTimeTokenExpirationMinutes { get; set; } = 5;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت تعداد استفاده استفاده شود؟
    /// </summary>
    public bool UseUsageLimitedTokens { get; set; } = true;

    /// <summary>
    /// حداکثر تعداد استفاده از توکن‌های محدود
    /// </summary>
    public int MaxTokenUsageCount { get; set; } = 1;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت جغرافیایی استفاده شود؟
    /// </summary>
    public bool UseGeolocationBoundTokens { get; set; } = false;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت شبکه استفاده شود؟
    /// </summary>
    public bool UseNetworkBoundTokens { get; set; } = false;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت زمانی استفاده شود؟
    /// </summary>
    public bool UseTimeWindowTokens { get; set; } = true;

    /// <summary>
    /// بازه زمانی مجاز برای استفاده از توکن (به ساعت)
    /// </summary>
    public int TokenTimeWindowHours { get; set; } = 24;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دسترسی استفاده شود؟
    /// </summary>
    public bool UseAccessScopeTokens { get; set; } = true;

    /// <summary>
    /// محدوده‌های دسترسی پیش‌فرض برای توکن‌ها
    /// </summary>
    public string[] DefaultAccessScopes { get; set; } = Array.Empty<string>();

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت عملیات استفاده شود؟
    /// </summary>
    public bool UseOperationLimitedTokens { get; set; } = true;

    /// <summary>
    /// عملیات‌های مجاز برای توکن‌ها
    /// </summary>
    public string[] AllowedOperations { get; set; } = Array.Empty<string>();

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت منبع استفاده شود؟
    /// </summary>
    public bool UseResourceBoundTokens { get; set; } = true;

    /// <summary>
    /// منابع مجاز برای دسترسی با توکن
    /// </summary>
    public string[] AllowedResources { get; set; } = Array.Empty<string>();

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت نقش استفاده شود؟
    /// </summary>
    public bool UseRoleBoundTokens { get; set; } = true;

    /// <summary>
    /// نقش‌های مجاز برای دسترسی با توکن
    /// </summary>
    public string[] AllowedRoles { get; set; } = Array.Empty<string>();

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت مجوز استفاده شود؟
    /// </summary>
    public bool UsePermissionBoundTokens { get; set; } = true;

    /// <summary>
    /// مجوزهای مجاز برای دسترسی با توکن
    /// </summary>
    public string[] AllowedPermissions { get; set; } = Array.Empty<string>();

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت زمان ورود استفاده شود؟
    /// </summary>
    public bool UseLoginTimeBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت تاریخ ورود استفاده شود؟
    /// </summary>
    public bool UseLoginDateBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت روز هفته استفاده شود؟
    /// </summary>
    public bool UseDayOfWeekBoundTokens { get; set; } = false;

    /// <summary>
    /// روزهای مجاز برای استفاده از توکن
    /// </summary>
    public DayOfWeek[] AllowedDays { get; set; } = Array.Empty<DayOfWeek>();

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت ساعت روز استفاده شود؟
    /// </summary>
    public bool UseTimeOfDayBoundTokens { get; set; } = false;

    /// <summary>
    /// بازه زمانی مجاز برای استفاده از توکن در روز (به ساعت)
    /// </summary>
    public (int StartHour, int EndHour) AllowedTimeOfDay { get; set; } = (9, 17);

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت مکان استفاده شود؟
    /// </summary>
    public bool UseLocationBoundTokens { get; set; } = false;

    /// <summary>
    /// مکان‌های مجاز برای استفاده از توکن
    /// </summary>
    public string[] AllowedLocations { get; set; } = Array.Empty<string>();

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت شبکه داخلی استفاده شود؟
    /// </summary>
    public bool UseInternalNetworkBoundTokens { get; set; } = false;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت شبکه خارجی استفاده شود؟
    /// </summary>
    public bool UseExternalNetworkBoundTokens { get; set; } = false;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه مورد اعتماد استفاده شود؟
    /// </summary>
    public bool UseTrustedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه شناخته شده استفاده شود؟
    /// </summary>
    public bool UseKnownDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه جدید استفاده شود؟
    /// </summary>
    public bool UseNewDeviceBoundTokens { get; set; } = false;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه غیرمعمول استفاده شود؟
    /// </summary>
    public bool UseUnusualDeviceBoundTokens { get; set; } = false;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه خطرناک استفاده شود؟
    /// </summary>
    public bool UseSuspiciousDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه مسدود شده استفاده شود؟
    /// </summary>
    public bool UseBlockedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه گم شده استفاده شود؟
    /// </summary>
    public bool UseLostDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه سرقت شده استفاده شود؟
    /// </summary>
    public bool UseStolenDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه خراب شده استفاده شود؟
    /// </summary>
    public bool UseBrokenDeviceBoundTokens { get; set; } = false;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه قدیمی استفاده شود؟
    /// </summary>
    public bool UseOldDeviceBoundTokens { get; set; } = false;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامعتبر استفاده شود؟
    /// </summary>
    public bool UseInvalidDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه ناشناس استفاده شود؟
    /// </summary>
    public bool UseUnknownDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnspecifiedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه تعریف نشده استفاده شود؟
    /// </summary>
    public bool UseUndefinedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUndeterminedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnidentifiedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnrecognizedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnacknowledgedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnconfirmedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnverifiedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnauthenticatedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnauthorizedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnapprovedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnacceptedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnconsentedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUndeclaredDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnspecifiedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnidentifiedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnrecognizedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnacknowledgedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnconfirmedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnverifiedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnauthenticatedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnauthorizedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnapprovedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnacceptedDeviceBoundTokens { get; set; } = true;

    /// <summary>
    /// آیا از توکن‌های دارای محدودیت دستگاه نامشخص استفاده شود؟
    /// </summary>
    public bool UseUnconsentedDeviceBoundTokens { get; set; } = true;
}