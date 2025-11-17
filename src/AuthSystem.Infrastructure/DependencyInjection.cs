using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Application.Common.Abstractions.Diagnostics;
using AuthSystem.Application.Common.Abstractions.DomainEvents;
using AuthSystem.Application.Common.Abstractions.Identity;
using AuthSystem.Application.Common.Abstractions.Messaging;
using AuthSystem.Application.Common.Abstractions.Monitoring;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Abstractions.Time;
using AuthSystem.Infrastructure.Auth;
using AuthSystem.Infrastructure.Diagnostics;
using AuthSystem.Infrastructure.DomainEvents;
using AuthSystem.Infrastructure.Extensions;
using AuthSystem.Infrastructure.Identity;
using AuthSystem.Infrastructure.Messaging.Email;
using AuthSystem.Infrastructure.Messaging.Sms;
using AuthSystem.Infrastructure.Options;
using AuthSystem.Infrastructure.Persistence.Sql;
using AuthSystem.Infrastructure.Persistence.Sql.Repositories;
using AuthSystem.Infrastructure.Security;
using AuthSystem.Infrastructure.SecurityEvents;
using AuthSystem.Infrastructure.Time;
using AuthSystem.Infrastructure.Verification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;

namespace AuthSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<TokenOptions>(configuration.GetSection(TokenOptions.SectionName));
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
        services.Configure<SmsOptions>(configuration.GetSection(SmsOptions.SectionName));
        services.Configure<SecurityWebhookOptions>(configuration.GetSection(SecurityWebhookOptions.SectionName));

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
        services.AddScoped<ITenantProvider, TenantProvider>();
        services.AddScoped<IRequestChannelProvider, HttpRequestChannelProvider>();
        services.AddScoped<IJitProvisioningService, JitProvisioningService>();
        services.AddScoped<IScimUserService, ScimUserService>();
        services.AddScoped<IPermissionService, PermissionService>();

        services.AddScoped<IDomainEventCollector, InMemoryDomainEventCollector>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IApplicationDbContext, SqlApplicationDbContext>();

        services.AddSingleton<IPasswordHasher, AspNetPasswordHasher>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<ISmtpClientFactory, DefaultSmtpClientFactory>();

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<RefreshTokenHasher>();

        services.AddHttpClient("sms", (provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<SmsOptions>>().Value;

            client.Timeout = TimeSpan.FromSeconds(Math.Max(1, options.TimeoutSeconds));

            if (Uri.TryCreate(options.Endpoint, UriKind.Absolute, out var absoluteEndpoint))
            {
                client.BaseAddress = absoluteEndpoint;
            }


            foreach (var header in options.Headers)
            {
                client.DefaultRequestHeaders.Remove(header.Key);
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        })
            .AddPolicyHandler((services, _) => CreateRetryPolicy(services));

        services.AddHttpClient("security-webhooks", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        services.AddScoped<NoOpEmailSender>();
        services.AddScoped<SmtpEmailSender>();
        services.AddScoped<IEmailSender>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<EmailOptions>>().Value;

            return string.Equals(options.Provider, "smtp", StringComparison.OrdinalIgnoreCase)
                ? provider.GetRequiredService<SmtpEmailSender>()
                : provider.GetRequiredService<NoOpEmailSender>();
        });

        services.AddScoped<NoOpSmsSender>();
        services.AddScoped<ProviderSmsSender>();
        services.AddScoped<ISmsSender>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<SmsOptions>>().Value;

            return string.Equals(options.Provider, "http", StringComparison.OrdinalIgnoreCase)
                ? provider.GetRequiredService<ProviderSmsSender>()
                : provider.GetRequiredService<NoOpSmsSender>();
        });

        services.AddScoped<WebhookSecurityEventPublisher>();
        services.AddScoped<ISecurityEventPublisher>(provider => provider.GetRequiredService<WebhookSecurityEventPublisher>());
        services.AddScoped<ISecurityEventReader, SecurityEventReader>();

        services.AddScoped<IVerificationCodeService, SqlVerificationCodeService>();

        services.AddInfraHealthChecks();

        return services;
    }
    private static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<SmsOptions>>().Value;
        var retryCount = Math.Clamp(options.RetryCount, 0, 10);

        if (retryCount == 0)
        {
            return Policy.NoOpAsync<HttpResponseMessage>();
        }

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(response => (int)response.StatusCode >= 500)
            .WaitAndRetryAsync(retryCount, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }
}