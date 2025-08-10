using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Application.DTOs.Requests;

/// <summary>
/// مدل درخواست ورود به سیستم
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// آدرس ایمیل یا نام کاربری
    /// </summary>
    [Required(ErrorMessage = "ایمیل یا نام کاربری الزامی است")]
    public string UsernameOrEmail { get; set; } = null!;

    /// <summary>
    /// رمز عبور کاربر
    /// </summary>
    [Required(ErrorMessage = "رمز عبور الزامی است")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// کد احراز هویت دو مرحله‌ای (در صورت فعال بودن)
    /// </summary>
    [StringLength(6, MinimumLength = 6, ErrorMessage = "کد احراز هویت باید 6 رقم باشد")]
    public string? TwoFactorCode { get; set; }

    /// <summary>
    /// آیا کاربر می‌خواهد به صورت خودکار وارد شود؟ (مانند "مرا به خاطر بسپار")
    /// </summary>
    public bool RememberMe { get; set; }

    /// <summary>
    /// اطلاعات کلاینت (مثل نوع دستگاه یا IP)
    /// </summary>
    public string? ClientInfo { get; set; }
}