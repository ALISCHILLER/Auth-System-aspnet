using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        if (string.IsNullOrWhiteSpace(_options.Endpoint))
        {
            throw new InvalidOperationException("SMS endpoint must be configured.");
        }
        var client = httpClientFactory.CreateClient("sms");
        var payload = new Dictionary<string, object?>
        {
            ["to"] = phoneNumber,
            ["body"] = message,
            ["from"] = _options.FromNumber
        };

        var serializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, _options.Endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload, serializerOptions), Encoding.UTF8)
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


        if (!string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            request.Headers.TryAddWithoutValidation("X-Api-Key", _options.ApiKey);
        }

        foreach (var header in _options.Headers)
        {
            request.Headers.Remove(header.Key);
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        try
        {
            var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
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