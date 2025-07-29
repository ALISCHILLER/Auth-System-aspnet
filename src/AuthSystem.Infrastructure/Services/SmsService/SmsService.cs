using AuthSystem.Domain.Interfaces.Services;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure.Services.SmsService;

/// <summary>
/// پیاده‌سازی ISmsService
/// در محیط واقعی باید به یک سرویس پیامکی متصل شود
/// </summary>
public class SmsService : ISmsService
{
    /// <summary>
    /// ارسال یک پیامک
    /// </summary>
    /// <param name="to">شماره گیرنده</param>
    /// <param name="message">متن پیام</param>
    /// <returns>تسک</returns>
    public async Task SendSmsAsync(string to, string message)
    {
        // در محیط توسعه، فقط لاگ می‌کنیم
        // در محیط واقعی باید به یک سرویس پیامکی متصل شویم
        await Task.CompletedTask;
    }

    /// <summary>
    /// ارسال کد تأیید پیامک
    /// </summary>
    /// <param name="to">شماره گیرنده</param>
    /// <param name="code">کد تأیید</param>
    /// <returns>تسک</returns>
    public async Task SendVerificationCodeAsync(string to, string code)
    {
        var message = $"کد تأیید شما: {code}";
        await SendSmsAsync(to, message);
    }
}