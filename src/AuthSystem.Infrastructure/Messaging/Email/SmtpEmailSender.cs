using AuthSystem.Application.Common.Abstractions.Messaging;
using AuthSystem.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace AuthSystem.Infrastructure.Messaging.Email;

internal sealed class SmtpEmailSender(
    ISmtpClientFactory smtpClientFactory,
    IOptions<EmailOptions> options,
    ILogger<SmtpEmailSender> logger) : IEmailSender
{
    private readonly EmailOptions _options = options.Value;

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken)
    {
        using var client = smtpClientFactory.Create(_options);

        var fromAddress = new MailAddress(_options.From, _options.DisplayName ?? _options.From);

        using var message = new MailMessage(fromAddress, new MailAddress(to))
        {
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        try
        {
            await client.SendAsync(message, cancellationToken).ConfigureAwait(false);
        }
        catch (SmtpException exception)
        {
            logger.LogError(exception, "Failed to send email via SMTP to {Recipient}", to);
            throw;
        }
    }
}