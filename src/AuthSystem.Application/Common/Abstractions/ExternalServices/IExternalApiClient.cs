namespace AuthSystem.Application.Common.Abstractions.ExternalServices;

/// <summary>
/// سرویس ارتباط با API های خارجی
/// </summary>
public interface IExternalApiClient
{
    Task<string> GetAsync(string url, CancellationToken cancellationToken = default);
    Task<string> PostAsync(string url, object body, CancellationToken cancellationToken = default);
}
