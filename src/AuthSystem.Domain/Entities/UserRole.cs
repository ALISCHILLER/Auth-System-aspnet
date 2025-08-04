using System;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت ارتباط Many-to-Many بین User و Role
/// </summary>
public class UserRole
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// شناسه نقش
    /// </summary>
    public Guid RoleId { get; private set; }

    // Navigation Properties
    /// <summary>
    /// کاربر مربوطه
    /// </summary>
    public User User { get; private set; } = default!;

    /// <summary>
    /// نقش مربوطه
    /// </summary>
    public Role Role { get; private set; } = default!;

    // Required for EF Core
    private UserRole() { }

    // Factory method
    public static UserRole Create(Guid userId, Guid roleId)
    {
        return new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };
    }
}