using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Application.DTOs.Requests;

/// <summary>
/// مدل درخواست بازنشانی رمز عبور
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// آدرس ایمیل کاربر برای ارسال لینک بازنشانی
    /// </summary>
    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
    public string Email { get; set; } = null!;
}