using AuthSystem.Application.Abstractions;
using AuthSystem.Application.Common.Interfaces;
using AuthSystem.Infrastructure.DomainEvents;
using AuthSystem.Infrastructure.Identity;
using AuthSystem.Infrastructure.Messaging;
using AuthSystem.Infrastructure.Persistence.InMemory;
using AuthSystem.Infrastructure.Tokens;
using AuthSystem.Infrastructure.Verification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration? configuration = null)
    {
        services.AddSingleton<InMemoryDatabase>();

        services.AddScoped<IDomainEventCollector, InMemoryDomainEventCollector>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

        services.AddScoped<IUserRepository, InMemoryUserRepository>();
        services.AddScoped<IRoleRepository, InMemoryRoleRepository>();
        services.AddScoped<IApplicationDbContext, InMemoryApplicationDbContext>();

        services.AddScoped<IVerificationCodeService, InMemoryVerificationCodeService>();
        services.AddSingleton<IEmailSender, NoOpEmailSender>();
        services.AddSingleton<ISmsSender, NoOpSmsSender>();

        services.AddScoped<CurrentUserContext>();
        services.AddScoped<CurrentUserService>();
        services.AddScoped<ICurrentUserService>(sp => sp.GetRequiredService<CurrentUserService>());
        services.AddScoped<ICurrentUserContextAccessor>(sp => sp.GetRequiredService<CurrentUserService>());

        services.AddSingleton<ITokenService, InMemoryTokenService>();
        services.AddOptions<TokenOptions>();

        if (configuration is not null)
        {
            services.Configure<TokenOptions>(configuration.GetSection("Authentication:Tokens"));
        }
        else
        {
            services.Configure<TokenOptions>(_ => { });
        }

        return services;
    }
}