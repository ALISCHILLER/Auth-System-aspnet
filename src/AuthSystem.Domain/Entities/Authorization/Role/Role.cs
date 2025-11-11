using AuthSystem.Domain.Common.Base;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Entities.Authorization.Role.Events;
using AuthSystem.Domain.Entities.Authorization.Role.Rules;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
namespace AuthSystem.Domain.Entities.Authorization.Role;

/// <summary>
/// Aggregate root representing a role with permissions and assigned users.
/// </summary>
public class Role : AggregateRoot<Guid>
{

    private readonly List<RolePermission> _permissions = new();

    private readonly List<UserRole> _userRoles = new();



    private Role()
    {
    }


    public Role(Guid id, string name, string description, bool isDefault = false, bool isSystemRole = false) : base(id)
    {
        CheckRule(new RoleNameMustBeValidRule(name));
        CheckRule(new RoleDescriptionMustBeValidRule(description));

        ApplyRaise(new RoleCreatedEvent(id, name, description, isDefault, isSystemRole));
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
        if (string.Equals(Name, name, StringComparison.Ordinal))
        {
            return;
        }

        ApplyRaise(new RoleUpdatedEvent(Id, name, Description));
    }


    public void UpdateDescription(string description)
    {

        CheckRule(new RoleDescriptionMustBeValidRule(description));

        if (string.Equals(Description, description, StringComparison.Ordinal))
        {
            return;
        }
        ApplyRaise(new RoleUpdatedEvent(Id, Name, description));
    }


    public RolePermission AddPermission(PermissionType permissionType)
    {
        CheckRule(new RoleCannotHaveDuplicatePermissionsRule(_permissions.Select(p => p.PermissionType), permissionType));

        var permissionId = Guid.NewGuid();
        ApplyRaise(new RolePermissionAddedEvent(Id, permissionId, permissionType));
        return _permissions.Single(p => p.Id == permissionId);
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
          .Where(p => p.Id != permission.Id)
            .Select(p => p.PermissionType)
            .ToArray();
        CheckRule(new RoleMustHavePermissionsRule(remainingPermissions));

        ApplyRaise(new RolePermissionRemovedEvent(Id, permission.Id, permission.PermissionType));
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
        var assignment = new UserRole(Guid.NewGuid(), userId, username, Id, Name, DomainClock.Instance.UtcNow);
        _userRoles.Add(assignment);
        MarkAsUpdated(occurredOn: assignment.AssignedAt);
        return assignment;
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


        ApplyRaise(new RoleDeletedEvent(Id));
    }


    public void Restore()
    {
        if (!IsDeleted)
        {
            return;
        }

        ApplyRaise(new RoleUndeletedEvent(Id));
    }


    private void On(RoleCreatedEvent @event)
    {
        Id = @event.RoleId;
        Name = @event.Name;
        Description = @event.Description;
        IsDefault = @event.IsDefault;
        IsSystemRole = @event.IsSystemRole;
        _permissions.Clear();
        _userRoles.Clear();
        MarkAsCreated(occurredOn: @event.OccurredOn);
    }

    private void On(RoleUpdatedEvent @event)
    {
        Name = @event.Name;
        Description = @event.Description;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }


    private void On(RolePermissionAddedEvent @event)
    {
        var permission = new RolePermission(@event.RolePermissionId, @event.RoleId, @event.PermissionType, @event.OccurredOn);
        _permissions.Add(permission);
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(RolePermissionRemovedEvent @event)
    {
        var permission = _permissions.FirstOrDefault(p => p.Id == @event.RolePermissionId);
        if (permission is not null)
        {
            _permissions.Remove(permission);
        }

        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }


    private void On(RoleDeletedEvent @event)
    {
        MarkAsDeleted(occurredOn: @event.OccurredOn);
    }

    private void On(RoleUndeletedEvent @event)
    {
        ClearDeletion(occurredOn: @event.OccurredOn);
    }

}