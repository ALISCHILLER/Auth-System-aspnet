using AuthSystem.Infrastructure.Options;
using System.Net;
using System.Net.Mail;

namespace AuthSystem.Infrastructure.Messaging.Email;

internal sealed class SmtpClientAdapter : ISmtpClient
{
    private readonly SmtpClient _innerClient;

    public SmtpClientAdapter(EmailOptions options)
    {
        _innerClient = new SmtpClient(options.Host, options.Port)
        {
            EnableSsl = options.EnableSsl
        };

        if (!string.IsNullOrWhiteSpace(options.Username))
        {
            _innerClient.Credentials = new NetworkCredential(options.Username, options.Password);
        }
    }

    public Task SendAsync(MailMessage message, CancellationToken cancellationToken)
    {
        return _innerClient.SendMailAsync(message, cancellationToken);
    }

    public void Dispose()
    {
        _innerClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
