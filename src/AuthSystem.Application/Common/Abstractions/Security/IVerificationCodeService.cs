namespace AuthSystem.Application.Common.Abstractions.Security;

public interface IVerificationCodeService
{
    Task<string> IssueAsync(Guid userId, TimeSpan timeToLive, CancellationToken cancellationToken);
    Task<bool> ValidateAsync(Guid userId, string code, CancellationToken cancellationToken);
    Task InvalidateAsync(Guid userId, string code, CancellationToken cancellationToken);
}