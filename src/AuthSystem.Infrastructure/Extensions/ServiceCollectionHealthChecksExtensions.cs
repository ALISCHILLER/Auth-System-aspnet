using AuthSystem.Infrastructure.Persistence.Sql;
using Microsoft.Extensions.DependencyInjection;

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