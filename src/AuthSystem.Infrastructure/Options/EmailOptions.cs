using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Infrastructure.Options;

/// <summary>
/// تنظیمات مربوط به سرویس ایمیل
/// </summary>
public class EmailOptions
{
    public const string SectionName = "Email";

    /// <summary>
    /// آدرس سرور SMTP
    /// </summary>
    [Required(ErrorMessage = "Email:SmtpServer الزامی است.")]
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// پورت SMTP
    /// </summary>
    [Range(1, 65535, ErrorMessage = "Email:SmtpPort باید بین 1 تا 65535 باشد.")]
    public int SmtpPort { get; set; } = 587;

    /// <summary>
    /// نام کاربری SMTP
    /// </summary>
    [Required(ErrorMessage = "Email:SmtpUsername الزامی است.")]
    public string SmtpUsername { get; set; } = string.Empty;

    /// <summary>
    /// رمز عبور SMTP
    /// </summary>
    [Required(ErrorMessage = "Email:SmtpPassword الزامی است.")]
    public string SmtpPassword { get; set; } = string.Empty;

    /// <summary>
    /// آدرس ایمیل فرستنده
    /// </summary>
    [Required(ErrorMessage = "Email:FromEmail الزامی است.")]
    [EmailAddress(ErrorMessage = "Email:FromEmail نامعتبر است.")]
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// نام فرستنده
    /// </summary>
    [Required(ErrorMessage = "Email:FromName الزامی است.")]
    public string FromName { get; set; } = string.Empty;
}