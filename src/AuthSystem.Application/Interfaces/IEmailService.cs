using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Interfaces;

/// <summary>
/// واسط برای ارسال ایمیل
/// </summary>
public interface IEmailService
{
    Task SendWelcomeEmailAsync(string to, string userName, string culture, CancellationToken cancellationToken);
    Task SendEmailConfirmationEmailAsync(string to, string userName, string token, string culture, CancellationToken cancellationToken);
    Task SendPasswordResetEmailAsync(string to, string userName, string token, string culture, CancellationToken cancellationToken);
    Task SendPasswordChangedEmailAsync(string to, string userName, string culture, CancellationToken cancellationToken);
    Task SendAccountLockedEmailAsync(string to, string userName, string culture, CancellationToken cancellationToken);
}