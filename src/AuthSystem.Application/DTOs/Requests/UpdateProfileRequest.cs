using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Application.DTOs.Requests;

/// <summary>
/// مدل درخواست به‌روزرسانی پروفایل کاربر
/// </summary>
public class UpdateProfileRequest
{
    /// <summary>
    /// نام کاربری
    /// </summary>
    [StringLength(50, MinimumLength = 3, ErrorMessage = "نام کاربری باید بین 3 تا 50 کاراکتر باشد")]
    public string? UserName { get; set; }

    /// <summary>
    /// شماره تلفن
    /// </summary>
    [Phone(ErrorMessage = "فرمت شماره تلفن نامعتبر است")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// جنسیت کاربر
    /// </summary>
    [RegularExpression(@"^(Male|Female|Other)$", ErrorMessage = "جنسیت باید یکی از مقادیر Male, Female یا Other باشد")]
    public string? Gender { get; set; }

    /// <summary>
    /// فایل تصویر پروفایل (Base64 یا URL)
    /// </summary>
    public string? ProfileImage { get; set; }

    /// <summary>
    /// زبان یا فرهنگ کاربر (مثل fa-IR, en-US)
    /// </summary>
    [StringLength(10, ErrorMessage = "فرمت زبان نامعتبر است")]
    public string? Culture { get; set; }
}