using System.Threading.Tasks;

namespace AuthSystem.Domain.Interfaces.Services;

/// <summary>
/// رابط برای سرویس ارسال پیامک
/// این رابط روش‌های لازم برای ارسال پیامک را تعریف می‌کند
/// </summary>
public interface ISmsService
{
    /// <summary>
    /// ارسال یک پیامک
    /// </summary>
    /// <param name="to">شماره گیرنده</param>
    /// <param name="message">متن پیام</param>
    /// <returns>تسک</returns>
    Task SendSmsAsync(string to, string message);

    /// <summary>
    /// ارسال کد تأیید پیامک
    /// </summary>
    /// <param name="to">شماره گیرنده</param>
    /// <param name="code">کد تأیید</param>
    /// <returns>تسک</returns>
    Task SendVerificationCodeAsync(string to, string code);
}