namespace AuthSystem.Application.Contracts.Users;

/// <summary>
/// Represents a SCIM user resource payload for create or update operations.
/// </summary>
public sealed class ScimUserResource
{
    /// <summary>
    /// Gets or sets the unique username for the user.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Gets or sets the primary email for the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the given (first) name.
    /// </summary>
    public string? GivenName { get; set; }

    /// <summary>
    /// Gets or sets the family (last) name.
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// Gets or sets whether the user is active.
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// Gets or sets an optional tenant identifier associated with the user.
    /// </summary>
    public string? TenantId { get; set; }
}