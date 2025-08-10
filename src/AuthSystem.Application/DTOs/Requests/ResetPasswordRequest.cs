using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Application.DTOs.Requests;

/// <summary>
/// مدل درخواست بازنشانی رمز عبور (با توکن)
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// توکن ارسال شده از طریق ایمیل
    /// </summary>
    [Required(ErrorMessage = "توکن الزامی است")]
    public string Token { get; set; } = null!;

    /// <summary>
    /// آدرس ایمیل کاربر
    /// </summary>
    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// رمز عبور جدید
    /// </summary>
    [Required(ErrorMessage = "رمز عبور جدید الزامی است")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "رمز عبور جدید باید حداقل 8 کاراکتر باشد")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "رمز عبور جدید باید شامل حرف بزرگ، حرف کوچک، عدد و یک نماد خاص باشد")]
    public string NewPassword { get; set; } = null!;

    /// <summary>
    /// تکرار رمز عبور جدید
    /// </summary>
    [Required(ErrorMessage = "تکرار رمز عبور جدید الزامی است")]
    [Compare("NewPassword", ErrorMessage = "رمز عبور جدید و تکرار آن مطابقت ندارند")]
    public string ConfirmPassword { get; set; } = null!;
}