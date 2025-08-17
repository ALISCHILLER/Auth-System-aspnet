using System.ComponentModel;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// نتیجه تلاش برای ورود به سیستم
/// این enum تمام حالات ممکن برای نتیجه ورود را مشخص می‌کند
/// </summary>
public enum LoginResult
{
    /// <summary>
    /// ورود موفقیت‌آمیز بود
    /// کاربر احراز هویت شد و می‌تواند وارد سیستم شود
    /// </summary>
    [Description("ورود موفق")]
    Success = 1,

    /// <summary>
    /// نام کاربری یا ایمیل نامعتبر است
    /// کاربری با این مشخصات یافت نشد
    /// </summary>
    [Description("نام کاربری نامعتبر")]
    InvalidUsername = 2,

    /// <summary>
    /// رمز عبور اشتباه است
    /// کاربر یافت شد اما رمز عبور صحیح نیست
    /// </summary>
    [Description("رمز عبور اشتباه")]
    InvalidPassword = 3,

    /// <summary>
    /// حساب کاربری غیرفعال است
    /// کاربر نمی‌تواند وارد شود تا فعال شود
    /// </summary>
    [Description("حساب غیرفعال")]
    AccountDisabled = 4,

    /// <summary>
    /// حساب کاربری قفل شده است
    /// معمولاً به دلیل تلاش‌های ناموفق متعدد
    /// </summary>
    [Description("حساب قفل شده")]
    AccountLocked = 5,

    /// <summary>
    /// حساب کاربری هنوز تأیید نشده است
    /// کاربر باید ایمیل یا موبایل خود را تأیید کند
    /// </summary>
    [Description("حساب تأیید نشده")]
    AccountNotVerified = 6,

    /// <summary>
    /// نیاز به احراز هویت دو عاملی است
    /// کاربر باید کد دو عاملی را وارد کند
    /// </summary>
    [Description("نیاز به احراز هویت دو عاملی")]
    RequiresTwoFactor = 7,

    /// <summary>
    /// کد احراز هویت دو عاملی نامعتبر است
    /// </summary>
    [Description("کد دو عاملی نامعتبر")]
    InvalidTwoFactorCode = 8,

    /// <summary>
    /// IP کاربر مسدود شده است
    /// دسترسی از این IP مجاز نیست
    /// </summary>
    [Description("IP مسدود شده")]
    IpBlocked = 9,

    /// <summary>
    /// نیاز به تغییر رمز عبور است
    /// کاربر باید رمز عبور خود را تغییر دهد
    /// </summary>
    [Description("نیاز به تغییر رمز عبور")]
    PasswordChangeRequired = 10,

    /// <summary>
    /// حساب کاربری منقضی شده است
    /// </summary>
    [Description("حساب منقضی شده")]
    AccountExpired = 11,

    /// <summary>
    /// تلاش‌های زیادی برای ورود انجام شده
    /// کاربر باید مدتی صبر کند
    /// </summary>
    [Description("تلاش‌های زیاد")]
    TooManyAttempts = 12,

    /// <summary>
    /// مرورگر یا دستگاه نامعتبر است
    /// این دستگاه مجاز به ورود نیست
    /// </summary>
    [Description("دستگاه نامعتبر")]
    InvalidDevice = 13,

    /// <summary>
    /// ورود در این زمان مجاز نیست
    /// ممکن است محدودیت زمانی وجود داشته باشد
    /// </summary>
    [Description("زمان غیرمجاز")]
    TimeRestriction = 14,

    /// <summary>
    /// ورود از این مکان مجاز نیست
    /// محدودیت جغرافیایی وجود دارد
    /// </summary>
    [Description("مکان غیرمجاز")]
    LocationRestriction = 15,

    /// <summary>
    /// نیاز به Captcha است
    /// کاربر باید کد امنیتی را وارد کند
    /// </summary>
    [Description("نیاز به کپچا")]
    RequiresCaptcha = 16,

    /// <summary>
    /// Captcha نامعتبر است
    /// </summary>
    [Description("کپچا نامعتبر")]
    InvalidCaptcha = 17,

    /// <summary>
    /// نیاز به تأیید مدیر است
    /// ورود نیاز به تأیید مدیر دارد
    /// </summary>
    [Description("نیاز به تأیید مدیر")]
    RequiresAdminApproval = 18,

    /// <summary>
    /// خطای عمومی در فرآیند ورود
    /// </summary>
    [Description("خطای سیستمی")]
    SystemError = 19,

    /// <summary>
    /// نیاز به بازنشانی رمز عبور
    /// رمز عبور باید بازنشانی شود
    /// </summary>
    [Description("نیاز به بازنشانی رمز")]
    PasswordResetRequired = 20,

    /// <summary>
    /// ورود با رمز موقت انجام شد
    /// کاربر با رمز موقت وارد شده و باید آن را تغییر دهد
    /// </summary>
    [Description("ورود با رمز موقت")]
    TemporaryPasswordUsed = 21,

    /// <summary>
    /// حساب در حال بررسی است
    /// </summary>
    [Description("حساب در حال بررسی")]
    AccountUnderReview = 22,

    /// <summary>
    /// نیاز به پذیرش شرایط و ضوابط جدید
    /// </summary>
    [Description("نیاز به پذیرش شرایط")]
    RequiresTermsAcceptance = 23
}

/// <summary>
/// متدهای کمکی برای LoginResult
/// </summary>
public static class LoginResultExtensions
{
    /// <summary>
    /// بررسی موفقیت‌آمیز بودن ورود
    /// </summary>
    public static bool IsSuccessful(this LoginResult result)
    {
        return result == LoginResult.Success ||
               result == LoginResult.TemporaryPasswordUsed;
    }

    /// <summary>
    /// بررسی نیاز به اقدام اضافی
    /// </summary>
    public static bool RequiresAdditionalAction(this LoginResult result)
    {
        return result == LoginResult.RequiresTwoFactor ||
               result == LoginResult.RequiresCaptcha ||
               result == LoginResult.PasswordChangeRequired ||
               result == LoginResult.RequiresAdminApproval ||
               result == LoginResult.RequiresTermsAcceptance ||
               result == LoginResult.TemporaryPasswordUsed;
    }

    /// <summary>
    /// بررسی مسدود بودن دائمی
    /// </summary>
    public static bool IsPermanentlyBlocked(this LoginResult result)
    {
        return result == LoginResult.AccountDisabled ||
               result == LoginResult.IpBlocked ||
               result == LoginResult.AccountExpired;
    }

    /// <summary>
    /// بررسی مسدود بودن موقت
    /// </summary>
    public static bool IsTemporarilyBlocked(this LoginResult result)
    {
        return result == LoginResult.AccountLocked ||
               result == LoginResult.TooManyAttempts ||
               result == LoginResult.TimeRestriction ||
               result == LoginResult.LocationRestriction;
    }

    /// <summary>
    /// دریافت کد HTTP مناسب
    /// </summary>
    public static int GetHttpStatusCode(this LoginResult result)
    {
        return result switch
        {
            LoginResult.Success => 200,
            LoginResult.TemporaryPasswordUsed => 200,
            LoginResult.RequiresTwoFactor => 202,
            LoginResult.RequiresCaptcha => 202,
            LoginResult.RequiresTermsAcceptance => 202,
            LoginResult.InvalidUsername => 401,
            LoginResult.InvalidPassword => 401,
            LoginResult.InvalidTwoFactorCode => 401,
            LoginResult.InvalidCaptcha => 401,
            LoginResult.AccountLocked => 423,
            LoginResult.TooManyAttempts => 429,
            LoginResult.IpBlocked => 403,
            LoginResult.LocationRestriction => 403,
            LoginResult.TimeRestriction => 403,
            LoginResult.SystemError => 500,
            _ => 401
        };
    }

    /// <summary>
    /// دریافت پیام مناسب برای کاربر
    /// </summary>
    public static string GetUserMessage(this LoginResult result)
    {
        return result switch
        {
            LoginResult.Success => "ورود با موفقیت انجام شد.",
            LoginResult.InvalidUsername => "نام کاربری یا ایمیل اشتباه است.",
            LoginResult.InvalidPassword => "رمز عبور اشتباه است.",
            LoginResult.AccountDisabled => "حساب کاربری شما غیرفعال است. لطفاً با پشتیبانی تماس بگیرید.",
            LoginResult.AccountLocked => "حساب شما به دلیل تلاش‌های ناموفق متعدد قفل شده است.",
            LoginResult.AccountNotVerified => "لطفاً ابتدا حساب کاربری خود را تأیید کنید.",
            LoginResult.RequiresTwoFactor => "لطفاً کد احراز هویت دو عاملی را وارد کنید.",
            LoginResult.InvalidTwoFactorCode => "کد احراز هویت دو عاملی نامعتبر است.",
            LoginResult.IpBlocked => "دسترسی از این آدرس IP مسدود شده است.",
            LoginResult.PasswordChangeRequired => "لطفاً رمز عبور خود را تغییر دهید.",
            LoginResult.AccountExpired => "حساب کاربری شما منقضی شده است.",
            LoginResult.TooManyAttempts => "تلاش‌های زیادی انجام داده‌اید. لطفاً چند دقیقه صبر کنید.",
            LoginResult.InvalidDevice => "ورود از این دستگاه مجاز نیست.",
            LoginResult.TimeRestriction => "در این زمان امکان ورود وجود ندارد.",
            LoginResult.LocationRestriction => "ورود از این مکان مجاز نیست.",
            LoginResult.RequiresCaptcha => "لطفاً کد امنیتی را وارد کنید.",
            LoginResult.InvalidCaptcha => "کد امنیتی نامعتبر است.",
            LoginResult.RequiresAdminApproval => "ورود شما نیاز به تأیید مدیر دارد.",
            LoginResult.SystemError => "خطای سیستمی رخ داده است. لطفاً بعداً تلاش کنید.",
            LoginResult.PasswordResetRequired => "لطفاً رمز عبور خود را بازنشانی کنید.",
            LoginResult.TemporaryPasswordUsed => "با رمز موقت وارد شده‌اید. لطفاً رمز عبور خود را تغییر دهید.",
            LoginResult.AccountUnderReview => "حساب شما در حال بررسی است.",
            LoginResult.RequiresTermsAcceptance => "لطفاً شرایط و ضوابط جدید را بپذیرید.",
            _ => "خطای ناشناخته در ورود."
        };
    }
}
