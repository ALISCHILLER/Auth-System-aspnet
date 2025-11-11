using AuthSystem.Infrastructure.Options;

namespace AuthSystem.Infrastructure.Messaging.Email;

internal sealed class DefaultSmtpClientFactory : ISmtpClientFactory
{
    public ISmtpClient Create(EmailOptions options)
    {
        return new SmtpClientAdapter(options);
    }
}