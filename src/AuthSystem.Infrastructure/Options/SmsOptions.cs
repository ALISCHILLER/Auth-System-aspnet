using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Infrastructure.Options;

/// <summary>
/// تنظیمات مربوط به سرویس پیامک
/// </summary>
public class SmsOptions
{
    public const string SectionName = "Sms";

    /// <summary>
    /// آدرس API سرویس SMS
    /// </summary>
    [Required(ErrorMessage = "Sms:ApiUrl الزامی است.")]
    public string ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// کلید API
    /// </summary>
    [Required(ErrorMessage = "Sms:ApiKey الزامی است.")]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// شماره فرستنده
    /// </summary>
    [Required(ErrorMessage = "Sms:SenderNumber الزامی است.")]
    public string SenderNumber { get; set; } = string.Empty;
}