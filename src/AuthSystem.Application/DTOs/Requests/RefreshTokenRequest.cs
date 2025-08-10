using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Application.DTOs.Requests;

/// <summary>
/// مدل درخواست دریافت توکن جدید با توکن تازه‌سازی
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// توکن تازه‌سازی
    /// </summary>
    [Required(ErrorMessage = "توکن تازه‌سازی الزامی است")]
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// زبان یا فرهنگ کاربر (مثل fa-IR, en-US)
    /// </summary>
    [StringLength(10, ErrorMessage = "فرمت زبان نامعتبر است")]
    public string? Culture { get; set; }
}