using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Application.Common.Authorization;
using AuthSystem.Application.Common.Behaviors;
using AuthSystem.Application.Common.Options;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AuthSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        services.AddAutoMapper(assembly);

        services.AddScoped<ICurrentUserPermissionCache, CurrentUserPermissionCache>();

        services.AddOptions<PipelineLoggingOptions>();
        services.PostConfigure<PipelineLoggingOptions>(options =>
        {
            if (options.SlowRequestThresholdMilliseconds <= 0)
            {
                options.SlowRequestThresholdMilliseconds = 500;
            }
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        return services;
    }
}