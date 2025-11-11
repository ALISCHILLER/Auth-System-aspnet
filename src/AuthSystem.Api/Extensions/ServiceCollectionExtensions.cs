using AuthSystem.Api.RealTime;
using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

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
        services.AddInfraHealthChecks();
        return services;
    }
    public static IServiceCollection AddRealTimeSecurityEvents(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddScoped<SignalRSecurityEventPublisher>();
        services.AddScoped<ISecurityEventPublisher>(provider => provider.GetRequiredService<SignalRSecurityEventPublisher>());
        return services;
    }
}