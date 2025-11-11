namespace AuthSystem.Application.Common.Abstractions.Messaging;

public interface ISmsSender
{
    Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
}