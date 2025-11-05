using System;
using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Application.Common.Abstractions.DomainEvents;
using AuthSystem.Application.Common.Abstractions.Messaging;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Infrastructure.Auth;
using AuthSystem.Infrastructure.DomainEvents;
using AuthSystem.Infrastructure.Extensions;
using AuthSystem.Infrastructure.Identity;
using AuthSystem.Infrastructure.Messaging.Email;
using AuthSystem.Infrastructure.Messaging.Sms;
using AuthSystem.Infrastructure.Options;
using AuthSystem.Infrastructure.Persistence.Sql;
using AuthSystem.Infrastructure.Persistence.Sql.Repositories;
using AuthSystem.Infrastructure.Tokens;
using AuthSystem.Infrastructure.Verification;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<TokenOptions>(configuration.GetSection(TokenOptions.SectionName));
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
        services.Configure<SmsOptions>(configuration.GetSection(SmsOptions.SectionName));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string 'Default' is missing.");

            options.UseSqlServer(connectionString, sql =>
            {
                sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPermissionService, PermissionService>();

        services.AddScoped<IDomainEventCollector, InMemoryDomainEventCollector>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IApplicationDbContext, SqlApplicationDbContext>();


        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<RefreshTokenHasher>();


        services.AddScoped<IEmailSender, NoOpEmailSender>();
        services.AddScoped<ISmsSender, NoOpSmsSender>();

        services.AddScoped<IVerificationCodeService, SqlVerificationCodeService>();

        services.AddInfraHealthChecks();

        return services;
    }
}