namespace AuthSystem.Application.Common.Abstractions.ExternalServices;

/// <summary>
/// سرویس احراز هویت از طریق ارائه‌دهنده خارجی (Google, Facebook, ...)
/// </summary>
public interface IExternalAuthProvider
{
    Task<(string UserId, string Email)?> AuthenticateAsync(string accessToken, CancellationToken cancellationToken = default);
}
