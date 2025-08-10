using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Application.DTOs.Requests;

/// <summary>
/// مدل درخواست تغییر رمز عبور
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// رمز عبور فعلی کاربر
    /// </summary>
    [Required(ErrorMessage = "رمز عبور فعلی الزامی است")]
    public string CurrentPassword { get; set; } = null!;

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
    public string ConfirmNewPassword { get; set; } = null!;
}