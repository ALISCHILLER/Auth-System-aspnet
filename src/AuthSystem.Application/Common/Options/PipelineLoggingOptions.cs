namespace AuthSystem.Application.Common.Options;

public sealed class PipelineLoggingOptions
{
    public int SlowRequestThresholdMilliseconds { get; set; } = 500;
}