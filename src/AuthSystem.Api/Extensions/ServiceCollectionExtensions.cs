using AuthSystem.Api.RealTime;
using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Infrastructure.Extensions;
using AuthSystem.Infrastructure.Persistence.Sql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AuthSystem.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiVersioningAndExplorer(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IServiceCollection AddInfrastructureHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddDbContextCheck<ApplicationDbContext>("database");
        return services;
    }
    public static IServiceCollection AddRealTimeSecurityEvents(this IServiceCollection services)
    {
        services.AddSignalR();
        services.Decorate<ISecurityEventPublisher, SignalRSecurityEventPublisher>();
        return services;
    }
}