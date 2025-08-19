using System;
using AuthSystem.Domain.Common;
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
    public Guid UserId { get; }

    /// <summary>
    /// نام کاربری
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// شناسه نقش
    /// </summary>
    public Guid RoleId { get; }

    /// <summary>
    /// نام نقش
    /// </summary>
    public string RoleName { get; }

    /// <summary>
    /// تاریخ انتساب
    /// </summary>
    public DateTime AssignedAt { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private UserRole()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
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
        AssignedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// آیا این نقش مربوط به کاربر خاصی است
    /// </summary>
    public bool IsForUser(Guid userId)
    {
        return UserId == userId;
    }

    /// <summary>
    /// آیا این نقش مربوط به نقش خاصی است
    /// </summary>
    public bool IsForRole(Guid roleId)
    {
        return RoleId == roleId;
    }
}