using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Application.DTOs.Requests;

/// <summary>
/// مدل درخواست تأیید ایمیل
/// </summary>
public class ConfirmEmailRequest
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    [Required(ErrorMessage = "شناسه کاربر الزامی است")]
    public Guid UserId { get; set; }

    /// <summary>
    /// توکن تأیید ایمیل
    /// </summary>
    [Required(ErrorMessage = "توکن الزامی است")]
    public string Token { get; set; } = null!;

    /// <summary>
    /// زبان یا فرهنگ کاربر (مثل fa-IR, en-US)
    /// </summary>
    [StringLength(10, ErrorMessage = "فرمت زبان نامعتبر است")]
    public string? Culture { get; set; }
}