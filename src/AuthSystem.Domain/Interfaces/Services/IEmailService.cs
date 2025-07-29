using System.Threading.Tasks;

namespace AuthSystem.Domain.Interfaces.Services;

/// <summary>
/// رابط برای سرویس ارسال ایمیل
/// این رابط روش‌های لازم برای ارسال ایمیل را تعریف می‌کند
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// ارسال یک ایمیل
    /// </summary>
    /// <param name="to">گیرنده</param>
    /// <param name="subject">موضوع</param>
    /// <param name="body">متن</param>
    /// <param name="isHtml">آیا متن HTML است؟</param>
    /// <returns>تسک</returns>
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);

    /// <summary>
    /// ارسال ایمیل خوش‌آمدگویی
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="username">نام کاربری</param>
    /// <returns>تسک</returns>
    Task SendWelcomeEmailAsync(string to, string username);

    /// <summary>
    /// ارسال ایمیل بازیابی رمز عبور
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="resetLink">لینک بازیابی</param>
    /// <returns>تسک</returns>
    Task SendPasswordResetEmailAsync(string to, string resetLink);

    /// <summary>
    /// ارسال ایمیل تأیید ایمیل
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="confirmationToken">توکن تأیید</param>
    /// <returns>تسک</returns>
    Task SendEmailConfirmationEmailAsync(string to, string confirmationToken);
}