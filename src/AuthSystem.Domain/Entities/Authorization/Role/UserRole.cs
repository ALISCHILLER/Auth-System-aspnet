using System;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Common.Entities;

namespace AuthSystem.Domain.Entities.Authorization.Role;

/// <summary>
/// Entity linking a user to a role.
/// </summary>
public class UserRole : BaseEntity<Guid>
{

    public Guid UserId { get; private set; }

    
    public string Username { get; private set; } = default!;

  
    public Guid RoleId { get; private set; }

   
    public string RoleName { get; private set; } = default!;


    public DateTime AssignedAt { get; private set; }


    private UserRole()
    {
     
    }


    public UserRole(Guid id, Guid userId, string username, Guid roleId, string roleName, DateTime? assignedAt = null) : base(id)
    {
        UserId = userId;
        Username = username;
        RoleId = roleId;
        RoleName = roleName;
        AssignedAt = assignedAt?.ToUniversalTime() ?? DomainClock.Instance.UtcNow;
        MarkAsCreated(occurredOn: AssignedAt);
    }

    public bool IsForUser(Guid userId) => UserId == userId;

    public bool IsForRole(Guid roleId) => RoleId == roleId;
}