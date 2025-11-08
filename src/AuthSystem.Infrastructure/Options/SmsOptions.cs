using System;
using System.Collections.Generic;

namespace AuthSystem.Infrastructure.Options;

public sealed class SmsOptions
{
    public const string SectionName = "Sms";

    public string Provider { get; set; } = "noop";
    public string Endpoint { get; set; } = "https://example.com/api/sms";

    public string? ApiKey { get; set; }
        = null;
    public string? FromNumber { get; set; }
        = null;

    public IDictionary<string, string> Headers { get; set; }
        = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public int TimeoutSeconds { get; set; } = 30;

    public int RetryCount { get; set; } = 3;

}