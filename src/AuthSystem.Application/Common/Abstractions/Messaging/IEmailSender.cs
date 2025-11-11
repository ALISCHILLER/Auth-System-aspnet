namespace AuthSystem.Application.Common.Abstractions.Messaging;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}