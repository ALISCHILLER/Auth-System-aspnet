using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure.Messaging.Email;

internal interface ISmtpClient : IDisposable
{
    Task SendAsync(MailMessage message, CancellationToken cancellationToken);
}
