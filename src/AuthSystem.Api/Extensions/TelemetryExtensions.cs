using AuthSystem.Api.Options;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AuthSystem.Api.Extensions;

public static class TelemetryExtensions
{
    public static IServiceCollection AddApiTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelemetryOptions>(configuration.GetSection(TelemetryOptions.SectionName));

        var openTelemetry = services.AddOpenTelemetry();
        openTelemetry.ConfigureResource(resource => resource.AddService("AuthSystem.Api"));

        openTelemetry.WithMetrics((sp, metrics) =>
        {
            var options = sp.GetRequiredService<IOptions<TelemetryOptions>>().Value;
            if (!options.EnableMetrics)
            {
                return;
            }

            metrics.AddAspNetCoreInstrumentation();
            metrics.AddHttpClientInstrumentation();
            metrics.AddRuntimeInstrumentation();
            metrics.AddConsoleExporter();
        });

        openTelemetry.WithTracing((sp, tracing) =>
        {
            var options = sp.GetRequiredService<IOptions<TelemetryOptions>>().Value;
            if (!options.EnableTracing)
            {
                return;
            }

            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            tracing.AddConsoleExporter();
        });

        return services;
    }
}