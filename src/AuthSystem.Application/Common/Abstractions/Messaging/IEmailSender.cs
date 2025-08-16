namespace AuthSystem.Application.Common.Abstractions.Messaging;

/// <summary>
/// سرویس ارسال ایمیل
/// لایه Application فقط با این اینترفیس کار می‌کند و از جزئیات پیاده‌سازی خبر ندارد
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// ارسال یک ایمیل به آدرس مشخص
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="subject">موضوع ایمیل</param>
    /// <param name="body">متن ایمیل (HTML یا متن ساده)</param>
    /// <param name="isHtml">آیا متن ایمیل HTML است؟</param>
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true, CancellationToken cancellationToken = default);
}
