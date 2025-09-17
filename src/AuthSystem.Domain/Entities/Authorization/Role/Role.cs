using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Entities.Authorization.Role.Events;
using AuthSystem.Domain.Entities.Authorization.Role.Rules;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
namespace AuthSystem.Domain.Entities.Authorization.Role;

/// <summary>
/// ریشه تجمع نقش
/// </summary>
public class Role : AggregateRoot<Guid>
{
   
    /// </summary>
    private readonly List<RolePermission> _permissions = new();
   
    private readonly List<UserRole> _userRoles = new();
    


    private Role()
    {
    }


    public Role(
        Guid id,
        string name,
        string description,
        bool isDefault = false,
        bool isSystemRole = false) : base(id)
    {
        CheckRule(new RoleNameMustBeValidRule(name));
        CheckRule(new RoleDescriptionMustBeValidRule(description));

        Name = name;
        Description = description;
        IsDefault = isDefault;
        IsSystemRole = isSystemRole;
        ApplyRaise(new RoleCreatedEvent(Id, Name, Description, IsDefault, IsSystemRole));
    }
    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public bool IsDefault { get; private set; }

    public bool IsSystemRole { get; private set; }

    public IReadOnlyCollection<RolePermission> Permissions => _permissions.AsReadOnly();

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public void UpdateName(string name)
    {
        
        CheckRule(new RoleNameMustBeValidRule(name));

        Name = name;
        MarkAsUpdated();
        ApplyRaise(new RoleUpdatedEvent(Id, Name, Description));
    }

 
    public void UpdateDescription(string description)
    {
     
        CheckRule(new RoleDescriptionMustBeValidRule(description));

        Description = description;
        MarkAsUpdated();
        ApplyRaise(new RoleUpdatedEvent(Id, Name, Description));
    }

   
    public RolePermission AddPermission(PermissionType permissionType)
    {
        CheckRule(new RoleCannotHaveDuplicatePermissionsRule(_permissions.Select(p => p.PermissionType), permissionType));

        var permission = new RolePermission(Guid.NewGuid(), Id, permissionType);
        _permissions.Add(permission);
        MarkAsUpdated();

       
        ApplyRaise(new RolePermissionAddedEvent(Id, permissionType));

        return permission;
    }

    public void RemovePermission(PermissionType permissionType)
    {
        var permission = _permissions.FirstOrDefault(p => p.PermissionType == permissionType);

        if (permission is null)
        {
            throw InvalidUserRoleException.ForPermissionNotFound(Name, permissionType);
        }

        CheckRule(new SystemRoleCannotRemoveAdminPermissionRule(IsSystemRole, permissionType));
        var remainingPermissions = _permissions
           .Where(p => p.PermissionType != permissionType)
           .Select(p => p.PermissionType)
           .ToArray();
        CheckRule(new RoleMustHavePermissionsRule(remainingPermissions));

        _permissions.Remove(permission);
         MarkAsUpdated();

        ApplyRaise(new RolePermissionRemovedEvent(Id, permissionType));
    }

    public bool HasPermission(PermissionType permissionType) =>
      _permissions.Any(p => p.PermissionType == permissionType);

    public bool HasAllPermissions(IEnumerable<PermissionType> permissions) =>
        permissions.All(HasPermission);

    public bool HasAnyPermission(IEnumerable<PermissionType> permissions) =>
        permissions.Any(HasPermission);

    public IReadOnlyCollection<PermissionType> GetPermissions() =>
        _permissions.Select(p => p.PermissionType).ToArray();

    public UserRole AddUserToRole(Guid userId, string username)
    {
        CheckRule(new RoleCannotHaveDuplicateUsersRule(_userRoles.Select(ur => ur.UserId), userId));
        var userRole = new UserRole(Guid.NewGuid(), userId, username, Id, Name);
        _userRoles.Add(userRole);
        MarkAsUpdated();

        return userRole;
    }

    public void RemoveUserFromRole(Guid userId)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.UserId == userId);
        if (userRole is null)
        {
            throw InvalidUserRoleException.ForUserNotInRole(userId, Name);
        }
        var remainingUsers = _userRoles.Count - 1;
        CheckRule(new DefaultRoleCannotBeEmptyRule(IsDefault, remainingUsers));
        _userRoles.Remove(userRole);
        MarkAsUpdated();
    }

    public bool HasUser(Guid userId) => _userRoles.Any(ur => ur.UserId == userId);

    public int GetUserCount() => _userRoles.Count;

    public void Delete()
    {

        if (IsDeleted)
        {
            return;
        }

        MarkAsDeleted();
        ApplyRaise(new RoleDeletedEvent(Id));
    }


    public void Restore()
    {
        if (!IsDeleted)
        {
            return;
        }
        #region Event Handlers
        IsDeleted = false;
        DeletedAt = null;
        MarkAsUpdated();
        ApplyRaise(new RoleUndeletedEvent(Id));
    }
    #region Event Handlers

    private void On(RoleCreatedEvent @event)
    {
        Name = @event.Name;
        Description = @event.Description;
        IsDefault = @event.IsDefault;
        IsSystemRole = @event.IsSystemRole;
    }

    private void On(RoleUpdatedEvent @event)
    {
        Name = @event.Name;
        Description = @event.Description;
    }

 
    private void On(RolePermissionAddedEvent @event)
    {
        // تغییرات قبلاً اعمال شده است
    }

    private void On(RolePermissionRemovedEvent @event)
    {
        // تغییرات قبلاً اعمال شده است
    }

 
    private void On(RoleDeletedEvent @event)
    {
        // وضعیت حذف در متد Delete اعمال شده است
    }

    private void On(RoleUndeletedEvent @event)
    {
        // وضعیت بازگردانی در متد Restore اعمال شده است
    }

}