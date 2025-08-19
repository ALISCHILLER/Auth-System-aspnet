namespace AuthSystem.Domain.Enums;

/// <summary>
/// فرمت‌های کد تأیید
/// این enum تعیین می‌کند که کد تأیید به چه فرمتی تولید شود
/// </summary>
public enum CodeFormat
{
    /// <summary>
    /// کد تأیید فقط شامل اعداد
    /// </summary>
    Numeric = 1,

    /// <summary>
    /// کد تأیید شامل اعداد و حروف بزرگ
    /// </summary>
    Alphanumeric = 2,

    /// <summary>
    /// کد تأیید شامل اعداد، حروف بزرگ و کوچک
    /// </summary>
    CaseSensitive = 3,

    /// <summary>
    /// کد تأیید شامل اعداد و حروف فارسی
    /// </summary>
    Persian = 4,

    /// <summary>
    /// کد تأیید با فرمت QR Code
    /// </summary>
    QrCode = 5
}