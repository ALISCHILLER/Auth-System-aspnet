using AuthSystem.Shared.Constants;
using System.Security.Claims;

namespace AuthSystem.Api.Middleware;

/// <summary>
/// Resolves the tenant identifier from headers or claims and makes it available for downstream services.
/// </summary>
public sealed class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var tenantId = ResolveTenantId(context);
        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            context.Items[TenantConstants.HttpContextItemKey] = tenantId;
            EnsureTenantClaim(context, tenantId!);
        }

        await next(context).ConfigureAwait(false);
    }

    private static string? ResolveTenantId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(TenantConstants.HeaderName, out var headerTenant) && !string.IsNullOrWhiteSpace(headerTenant))
        {
            return headerTenant.ToString();
        }

        return context.User.FindFirst(TenantConstants.ClaimType)?.Value;
    }

    private static void EnsureTenantClaim(HttpContext context, string tenantId)
    {
        if (context.User.Identity is not ClaimsIdentity identity)
        {
            return;
        }

        if (identity.HasClaim(claim => claim.Type == TenantConstants.ClaimType))
        {
            return;
        }

        identity.AddClaim(new Claim(TenantConstants.ClaimType, tenantId));
    }
}