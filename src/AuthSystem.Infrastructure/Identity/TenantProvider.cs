using AuthSystem.Application.Common.Abstractions.Security;

namespace AuthSystem.Infrastructure.Identity;

internal sealed class TenantProvider(IHttpContextAccessor accessor) : ITenantProvider
{
    public string? TenantId
    {
        get
        {
            var context = accessor.HttpContext;
            if (context is null)
            {
                return null;
            }

            if (context.Items.TryGetValue(TenantConstants.HttpContextItemKey, out var tenant) && tenant is string tenantId)
            {
                return tenantId;
            }

            return context.User.FindFirst(TenantConstants.ClaimType)?.Value;
        }
    }
}