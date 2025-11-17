using AuthSystem.Api.Options;
using OpenTelemetry.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AuthSystem.Api.Extensions;

public static class TelemetryExtensions
{
    public static IServiceCollection AddApiTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelemetryOptions>(configuration.GetSection(TelemetryOptions.SectionName));

        var telemetryOptions = configuration.GetSection(TelemetryOptions.SectionName).Get<TelemetryOptions>() ?? new TelemetryOptions();
   
        var openTelemetry = services.AddOpenTelemetry();
        openTelemetry.ConfigureResource(resource => resource.AddService("AuthSystem.Api"));

        if (telemetryOptions.EnableMetrics)
        {
            openTelemetry.WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();
                metrics.AddRuntimeInstrumentation();
                metrics.AddConsoleExporter();
            });
        }

        if (telemetryOptions.EnableTracing)
        {
            openTelemetry.WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                tracing.AddConsoleExporter();
            });
        }

        return services;
    }
}