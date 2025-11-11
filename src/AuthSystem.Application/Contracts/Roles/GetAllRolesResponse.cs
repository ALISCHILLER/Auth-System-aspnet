namespace AuthSystem.Application.Contracts.Roles;

public sealed class GetAllRolesResponse
{
    public IReadOnlyCollection<RoleSummary> Roles { get; set; } = Array.Empty<RoleSummary>();

    public sealed class RoleSummary
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}