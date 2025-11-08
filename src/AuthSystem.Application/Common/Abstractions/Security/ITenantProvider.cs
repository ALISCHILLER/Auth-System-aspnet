namespace AuthSystem.Application.Common.Abstractions.Security;

public interface ITenantProvider
{
    string? TenantId { get; }
}