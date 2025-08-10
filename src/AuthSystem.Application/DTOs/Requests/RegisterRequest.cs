using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Application.DTOs.Requests;

/// <summary>
/// مدل درخواست ثبت‌نام کاربر جدید
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// نام کاربری منحصر به فرد
    /// </summary>
    [Required(ErrorMessage = "نام کاربری الزامی است")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "نام کاربری باید بین 3 تا 50 کاراکتر باشد")]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// آدرس ایمیل کاربر
    /// </summary>
    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
    [StringLength(256, ErrorMessage = "ایمیل نمی‌تواند بیش از 256 کاراکتر باشد")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// رمز عبور کاربر
    /// </summary>
    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "رمز عبور باید حداقل 8 کاراکتر باشد")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "رمز عبور باید شامل حرف بزرگ، حرف کوچک، عدد و یک نماد خاص باشد")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// تکرار رمز عبور (برای تأیید)
    /// </summary>
    [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
    [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند")]
    public string ConfirmPassword { get; set; } = null!;

    /// <summary>
    /// شماره تلفن کاربر
    /// </summary>
    [Phone(ErrorMessage = "فرمت شماره تلفن نامعتبر است")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// کد ملی کاربر
    /// </summary>
    [RegularExpression(@"^\d{10}$", ErrorMessage = "کد ملی باید دقیقاً 10 رقم باشد")]
    public string? NationalCode { get; set; }

    /// <summary>
    /// زبان یا فرهنگ کاربر (مثل fa-IR, en-US)
    /// </summary>
    [StringLength(10, ErrorMessage = "فرمت زبان نامعتبر است")]
    public string? Culture { get; set; }

    /// <summary>
    /// فایل تصویر پروفایل (Base64 یا URL)
    /// </summary>
    public string? ProfileImage { get; set; }
}