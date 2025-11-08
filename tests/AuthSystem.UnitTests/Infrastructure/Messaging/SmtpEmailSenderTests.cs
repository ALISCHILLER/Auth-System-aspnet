using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Messaging;
using AuthSystem.Infrastructure.Messaging.Email;
using AuthSystem.Infrastructure.Options;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace AuthSystem.UnitTests.Infrastructure.Messaging;

public class SmtpEmailSenderTests
{
    [Fact]
    public async Task SendAsync_Uses_Factory_And_Sends_Message()
    {
        var options = Options.Create(new EmailOptions
        {
            From = "no-reply@test.local",
            DisplayName = "Auth Test",
            Host = "smtp.test",
            Port = 2525,
            EnableSsl = true,
            Username = "user",
            Password = "pass"
        });
        var fakeClient = new FakeSmtpClient();
        var factory = new FakeSmtpClientFactory(fakeClient);
        IEmailSender sender = new SmtpEmailSender(factory, options, NullLogger<SmtpEmailSender>.Instance);

        await sender.SendAsync("user@example.com", "Subject", "<p>Hello</p>", CancellationToken.None);

        Assert.True(fakeClient.SendCalled);
        Assert.Equal("no-reply@test.local", fakeClient.FromAddress.Address);
        Assert.Equal("Auth Test", fakeClient.FromAddress.DisplayName);
        Assert.Equal("user@example.com", fakeClient.ToAddress.Address);
    }

    private sealed class FakeSmtpClientFactory(ISmtpClient smtpClient) : ISmtpClientFactory
    {
        public ISmtpClient Create(EmailOptions options) => smtpClient;
    }

    private sealed class FakeSmtpClient : ISmtpClient
    {
        public bool SendCalled { get; private set; }
        public MailAddress? FromAddress { get; private set; }
        public MailAddress? ToAddress { get; private set; }

        public Task SendAsync(MailMessage message, CancellationToken cancellationToken)
        {
            SendCalled = true;
            FromAddress = message.From;
            ToAddress = message.To[0];
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}