using System.Net.Mail;

namespace AuthSystem.Infrastructure.Messaging.Email;

internal interface ISmtpClient : IDisposable
{
    Task SendAsync(MailMessage message, CancellationToken cancellationToken);
}
