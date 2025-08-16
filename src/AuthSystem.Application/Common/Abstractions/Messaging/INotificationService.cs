namespace AuthSystem.Application.Common.Abstractions.Messaging;

/// <summary>
/// سرویس ارسال اعلان‌های چندکاناله (Push, Email, SMS)
/// </summary>
public interface INotificationService
{
    Task NotifyAsync(string userId, string title, string message, CancellationToken cancellationToken = default);
}
