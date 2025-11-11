using AuthSystem.Api.Options;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

namespace AuthSystem.Api.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddApiRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RateLimitingOptions>(configuration.GetSection(RateLimitingOptions.SectionName));

        services.AddRateLimiter((serviceProvider, options) =>
        {
            var rateOptions = serviceProvider.GetRequiredService<IOptions<RateLimitingOptions>>().Value;

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter("global", _ => CreateFixedWindowOptions(rateOptions.Global)));

            options.AddPolicy("auth-login", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    ResolvePartitionKey(context),
                    _ => CreateFixedWindowOptions(rateOptions.AuthLogin)));

            options.AddPolicy("auth-refresh", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    ResolvePartitionKey(context),
                    _ => CreateFixedWindowOptions(rateOptions.AuthRefresh)));

            options.AddPolicy("auth-logout", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    ResolvePartitionKey(context),
                    _ => CreateFixedWindowOptions(rateOptions.AuthLogout)));

            options.AddPolicy("two-factor-request", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    ResolvePartitionKey(context),
                    _ => CreateFixedWindowOptions(rateOptions.TwoFactorRequest)));

            options.AddPolicy("two-factor-verify", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    ResolvePartitionKey(context),
                    _ => CreateFixedWindowOptions(rateOptions.TwoFactorVerify)));
        });

        return services;
    }

    private static FixedWindowRateLimiterOptions CreateFixedWindowOptions(RateLimitingOptions.FixedWindowPolicyOptions options)
        => new()
        {
            PermitLimit = Math.Max(1, options.PermitLimit),
            Window = TimeSpan.FromSeconds(Math.Max(1, options.WindowSeconds)),
            QueueLimit = Math.Max(0, options.QueueLimit),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        };

    public static IApplicationBuilder UseApiRateLimiting(this IApplicationBuilder app)
        => app.UseRateLimiter();

    private static string ResolvePartitionKey(HttpContext context)
    {
        var tenant = context.Request.Headers.TryGetValue(TenantConstants.HeaderName, out var header)
            ? header.ToString()
            : context.Items.TryGetValue(TenantConstants.HttpContextItemKey, out var value) ? value?.ToString() : null;

        var ip = context.Connection.RemoteIpAddress?.ToString();
        var identity = context.User.Identity?.Name;

        var baseKey = !string.IsNullOrWhiteSpace(identity)
            ? identity!
            : !string.IsNullOrWhiteSpace(ip) ? ip! : "anonymous";

        return string.IsNullOrWhiteSpace(tenant) ? baseKey : $"{tenant}:{baseKey}";
    }
}