namespace AuthSystem.Application.Common.Abstractions.Messaging;

/// <summary>
/// سرویس ارسال پیامک
/// </summary>
public interface ISmsSender
{
    /// <summary>
    /// ارسال پیامک
    /// </summary>
    /// <param name="to">شماره گیرنده</param>
    /// <param name="message">متن پیامک</param>
    Task SendSmsAsync(string to, string message, CancellationToken cancellationToken = default);
}
