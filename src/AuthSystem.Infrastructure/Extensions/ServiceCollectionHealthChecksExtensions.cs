using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Infrastructure.Persistence.Sql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AuthSystem.Infrastructure.Extensions;

public static class ServiceCollectionHealthChecksExtensions
{
    public static IServiceCollection AddInfrastructureHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddDbContextCheck<ApplicationDbContext>("database");

        return services;
    }

    public static IServiceCollection AddInfraHealthChecks(this IServiceCollection services) =>
        services.AddInfrastructureHealthChecks();
}