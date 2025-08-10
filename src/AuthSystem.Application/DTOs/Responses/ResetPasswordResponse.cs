namespace AuthSystem.Application.DTOs.Responses;

/// <summary>
/// مدل پاسخ برای بازنشانی رمز عبور
/// </summary>
public class ResetPasswordResponse
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// آیا بازنشانی رمز عبور موفقیت‌آمیز بود؟
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// پیام نتیجه
    /// </summary>
    public string Message { get; set; } = null!;
}