using AuthSystem.Infrastructure.Options;

namespace AuthSystem.Infrastructure.Messaging.Email;

internal interface ISmtpClientFactory
{
    ISmtpClient Create(EmailOptions options);
}
