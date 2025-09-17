using System;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Common.Entities;

namespace AuthSystem.Domain.Entities.Authorization.Role;

/// <summary>
/// Entity برای نقش‌های کاربر
/// </summary>
public class UserRole : BaseEntity<Guid>
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// نام کاربری
    /// </summary>
    public string Username { get; private set; } = default!;

    /// <summary>
    /// شناسه نقش
    /// </summary>
    public Guid RoleId { get; private set; }

    /// <summary>
    /// نام نقش
    /// </summary>
    public string RoleName { get; private set; } = default!;

    /// <summary>
    /// تاریخ انتساب
    /// </summary>
    public DateTime AssignedAt { get; private set; }


    private UserRole()
    {
        // برای EF Core
    }

   
    public UserRole(
        Guid id,
        Guid userId,
        string username,
        Guid roleId,
        string roleName) : base(id)
    {
        UserId = userId;
        Username = username;
        RoleId = roleId;
        RoleName = roleName;
        AssignedAt = DomainClock.Instance.UtcNow;
    }

    public bool IsForUser(Guid userId) => UserId == userId;

    public bool IsForRole(Guid roleId) => RoleId == roleId;
}