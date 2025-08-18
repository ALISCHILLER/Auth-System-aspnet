// File: AuthSystem.Domain/Enums/CodeFormat.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// فرمت‌های مختلف کد تأیید
/// - این enum برای تعیین فرمت کد تأیید استفاده می‌شود
/// </summary>
public enum CodeFormat
{
    /// <summary>
    /// کد عددی (مثلاً 123456)
    /// </summary>
    Numeric = 1,

    /// <summary>
    /// کد الفبایی-عددی (مثلاً A1B2C3)
    /// </summary>
    Alphanumeric = 2,

    /// <summary>
    /// کد فقط حروف بزرگ (مثلاً ABC123)
    /// </summary>
    UppercaseLetters = 3,

    /// <summary>
    /// کد فقط اعداد (مثلاً 123456)
    /// </summary>
    NumbersOnly = 4,

    /// <summary>
    /// کد QR (برای احراز هویت دو عاملی)
    /// </summary>
    QRCode = 5
}