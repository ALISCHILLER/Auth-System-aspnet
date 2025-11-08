namespace AuthSystem.Api.Options;

public sealed class TelemetryOptions
{
    public const string SectionName = "Telemetry";

    public bool EnableTracing { get; set; } = true;

    public bool EnableMetrics { get; set; } = true;
}