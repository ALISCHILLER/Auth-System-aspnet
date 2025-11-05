using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Messaging;
using AuthSystem.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthSystem.Infrastructure.Messaging.Sms;

internal sealed class ProviderSmsSender(
    IHttpClientFactory httpClientFactory,
    IOptions<SmsOptions> options,
    ILogger<ProviderSmsSender> logger) : ISmsSender
{
    private readonly SmsOptions _options = options.Value;

    public async Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("sms");
        var payload = new
        {
            to = phoneNumber,
            body = message,
            apiKey = _options.ApiKey
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync(_options.Endpoint, content, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("SMS provider returned status code {StatusCode}", response.StatusCode);
            }
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Failed to send SMS to {PhoneNumber}", phoneNumber);
            throw;
        }
    }
}