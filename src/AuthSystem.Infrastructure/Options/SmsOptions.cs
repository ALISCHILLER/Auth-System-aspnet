namespace AuthSystem.Infrastructure.Options;

public sealed class SmsOptions
{
    public string Provider { get; set; } = "noop";
    public string? ApiKey { get; set; }
        = null;
    public string? Sender { get; set; }
        = null;
}