using AuthSystem.Infrastructure.Options;
using System.Net.Http;
using System.Net.Mail;

namespace AuthSystem.Infrastructure.Messaging.Email;

internal sealed class DefaultSmtpClientFactory : ISmtpClientFactory
{
    public ISmtpClient Create(EmailOptions options)
    {
        return new SmtpClientAdapter(options);
    }
}