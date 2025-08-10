namespace AuthSystem.Application.DTOs.Responses;

/// <summary>
/// مدل پاسخ برای تأیید ایمیل یا شماره تلفن
/// </summary>
public class ConfirmationResponse
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// آیا تأیید موفقیت‌آمیز بود؟
    /// </summary>
    public bool IsConfirmed { get; set; }

    /// <summary>
    /// نوع تأیید (ایمیل یا شماره تلفن)
    /// </summary>
    public string ConfirmationType { get; set; } = null!;

    /// <summary>
    /// پیام نتیجه
    /// </summary>
    public string Message { get; set; } = null!;
}