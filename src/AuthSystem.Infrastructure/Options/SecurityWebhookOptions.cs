namespace AuthSystem.Infrastructure.Options;

public sealed class SecurityWebhookOptions
{
    public const string SectionName = "SecurityWebhooks";

    public bool Enabled { get; set; }

    public IList<SecurityWebhookSubscription> Subscriptions { get; init; } = new List<SecurityWebhookSubscription>();
}

public sealed class SecurityWebhookSubscription
{
    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public IList<string> EventTypes { get; init; } = new List<string>();

    public string? Secret { get; set; }

    public IDictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();
}