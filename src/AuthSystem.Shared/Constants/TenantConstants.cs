namespace AuthSystem.Shared.Constants;

public static class TenantConstants
{
    public const string HeaderName = "X-Tenant-Id";
    public const string ClaimType = "tenant";
    public const string HttpContextItemKey = "tenantId";
}