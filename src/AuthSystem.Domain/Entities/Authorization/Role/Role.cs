using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Common.Exceptions;
using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;
using AuthSystem.Domain.Entities.Authorization.Role.Events;
using AuthSystem.Domain.Entities.Authorization.Role.Rules;

namespace AuthSystem.Domain.Entities.Authorization.Role;

/// <summary>
/// Aggregate Root برای نقش‌ها
/// این کلاس مسئول مدیریت نقش‌ها و مجوزهای آن‌ها است
/// </summary>
public class Role : AggregateRoot<Guid>
{
    /// <summary>
    /// نام نقش
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// توضیحات نقش
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// آیا نقش پیش‌فرض است
    /// </summary>
    public bool IsDefault { get; private set; }

    /// <summary>
    /// آیا نقش سیستمی است (غیرقابل حذف)
    /// </summary>
    public bool IsSystemRole { get; private set; }

    /// <summary>
    /// تاریخ ایجاد
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// تاریخ آخرین به‌روزرسانی
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// مجوزهای نقش
    /// </summary>
    private readonly List<RolePermission> _permissions = new();
    public IReadOnlyList<RolePermission> Permissions => _permissions.AsReadOnly();

    /// <summary>
    /// کاربران با این نقش
    /// </summary>
    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyList<UserRole> UserRoles => _userRoles.AsReadOnly();

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private Role()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    public Role(
        Guid id,
        string name,
        string description,
        bool isDefault = false,
        bool isSystemRole = false) : base(id)
    {
        // بررسی قوانین دامنه قبل از ایجاد
        CheckRule(new RoleNameMustBeValidRule(name));
        CheckRule(new RoleDescriptionMustBeValidRule(description));

        Name = name;
        Description = description;
        IsDefault = isDefault;
        IsSystemRole = isSystemRole;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    /// <summary>
    /// به‌روزرسانی نام
    /// </summary>
    public void UpdateName(string name)
    {
        // بررسی قانون دامنه
        CheckRule(new RoleNameMustBeValidRule(name));

        Name = name;
        UpdatedAt = DateTime.UtcNow;
        ApplyRaise(new RoleNameUpdatedEvent(Id, Name));
    }

    /// <summary>
    /// به‌روزرسانی توضیحات
    /// </summary>
    public void UpdateDescription(string description)
    {
        // بررسی قانون دامنه
        CheckRule(new RoleDescriptionMustBeValidRule(description));

        Description = description;
        UpdatedAt = DateTime.UtcNow;
        ApplyRaise(new RoleDescriptionUpdatedEvent(Id, Description));
    }

    /// <summary>
    /// افزودن مجوز جدید
    /// </summary>
    public RolePermission AddPermission(PermissionType permissionType)
    {
        // بررسی قوانین دامنه
        CheckRule(new RoleCannotHaveDuplicatePermissionsRule(this, permissionType));
        CheckRule(new SystemRoleCannotRemoveAdminPermissionRule(this, permissionType));

        var permission = new RolePermission(
            Guid.NewGuid(),
            Id,
            permissionType);

        _permissions.Add(permission);
        UpdatedAt = DateTime.UtcNow;

        // انتشار رویداد
        ApplyRaise(new RolePermissionAddedEvent(Id, permissionType));

        return permission;
    }

    /// <summary>
    /// حذف مجوز
    /// </summary>
    public void RemovePermission(PermissionType permissionType)
    {
        var permission = _permissions.FirstOrDefault(p => p.PermissionType == permissionType);
        if (permission == null)
            throw new RolePermissionNotFoundException($"مجوز '{permissionType}' یافت نشد");

        // بررسی قوانین دامنه
        CheckRule(new SystemRoleCannotRemoveAdminPermissionRule(this, permissionType));

        _permissions.Remove(permission);
        UpdatedAt = DateTime.UtcNow;

        // انتشار رویداد
        ApplyRaise(new RolePermissionRemovedEvent(Id, permissionType));
    }

    /// <summary>
    /// بررسی داشتن مجوز
    /// </summary>
    public bool HasPermission(PermissionType permissionType)
    {
        return _permissions.Any(p => p.PermissionType == permissionType);
    }

    /// <summary>
    /// بررسی داشتن همه مجوزها
    /// </summary>
    public bool HasAllPermissions(IEnumerable<PermissionType> permissionTypes)
    {
        return permissionTypes.All(HasPermission);
    }

    /// <summary>
    /// بررسی داشتن حداقل یک مجوز
    /// </summary>
    public bool HasAnyPermission(IEnumerable<PermissionType> permissionTypes)
    {
        return permissionTypes.Any(HasPermission);
    }

    /// <summary>
    /// دریافت مجوزها
    /// </summary>
    public IReadOnlyList<PermissionType> GetPermissions()
    {
        return _permissions
            .Select(p => p.PermissionType)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// افزودن کاربر به نقش
    /// </summary>
    public UserRole AddUserToRole(Guid userId, string username)
    {
        // بررسی قوانین دامنه
        CheckRule(new RoleCannotHaveDuplicateUsersRule(this, userId));
        CheckRule(new DefaultRoleCannotBeEmptyRule(this, userId));

        var userRole = new UserRole(
            Guid.NewGuid(),
            userId,
            username,
            Id,
            Name);

        _userRoles.Add(userRole);
        UpdatedAt = DateTime.UtcNow;

        // انتشار رویداد
        ApplyRaise(new UserRoleAddedEvent(Id, userId, username));

        return userRole;
    }

    /// <summary>
    /// حذف کاربر از نقش
    /// </summary>
    public void RemoveUserFromRole(Guid userId)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.UserId == userId);
        if (userRole == null)
            throw new UserRoleNotFoundException($"کاربر با شناسه {userId} در این نقش یافت نشد");

        // بررسی قوانین دامنه
        CheckRule(new DefaultRoleCannotBeEmptyRule(this, userId));

        _userRoles.Remove(userRole);
        UpdatedAt = DateTime.UtcNow;

        // انتشار رویداد
        ApplyRaise(new UserRoleRemovedEvent(Id, userId, userRole.Username));
    }

    /// <summary>
    /// بررسی داشتن کاربر
    /// </summary>
    public bool HasUser(Guid userId)
    {
        return _userRoles.Any(ur => ur.UserId == userId);
    }

    /// <summary>
    /// تعداد کاربران
    /// </summary>
    public int GetUserCount()
    {
        return _userRoles.Count;
    }

    /// <summary>
    /// تأیید صحت نقش
    /// </summary>
    public void Validate()
    {
        // بررسی قوانین دامنه
        CheckRule(new RoleNameMustBeValidRule(Name));
        CheckRule(new RoleDescriptionMustBeValidRule(Description));
    }

    /// <summary>
    /// ایجاد نقش ادمین پیش‌فرض
    /// </summary>
    public static Role CreateAdminRole()
    {
        var role = new Role(
            Guid.Parse("A0000000-0000-0000-0000-000000000001"),
            "Admin",
            "نقش ادمین سیستم با دسترسی کامل",
            isDefault: false,
            isSystemRole: true);

        // افزودن تمام مجوزها
        foreach (PermissionType permission in Enum.GetValues(typeof(PermissionType)))
        {
            role.AddPermission(permission);
        }

        return role;
    }

    /// <summary>
    /// ایجاد نقش کاربر پیش‌فرض
    /// </summary>
    public static Role CreateDefaultUserRole()
    {
        var role = new Role(
            Guid.Parse("U0000000-0000-0000-0000-000000000001"),
            "User",
            "نقش پیش‌فرض کاربران",
            isDefault: true,
            isSystemRole: true);

        // افزودن مجوزهای پایه
        role.AddPermission(PermissionType.Read);
        role.AddPermission(PermissionType.Create);

        return role;
    }

    #region Event Handlers

    /// <summary>
    /// پردازش رویداد به‌روزرسانی نام نقش
    /// </summary>
    private void On(RoleNameUpdatedEvent @event)
    {
        // منطق داخلی برای پردازش رویداد
        // اینجا معمولاً نیازی به تغییر حالت نیست
        // چون تغییرات قبلاً اعمال شده‌اند
    }

    /// <summary>
    /// پردازش رویداد به‌روزرسانی توضیحات نقش
    /// </summary>
    private void On(RoleDescriptionUpdatedEvent @event)
    {
        // منطق داخلی برای پردازش رویداد
    }

    /// <summary>
    /// پردازش رویداد افزودن مجوز
    /// </summary>
    private void On(RolePermissionAddedEvent @event)
    {
        // معمولاً نیازی به تغییر حالت اینجا نیست
        // چون تغییرات قبلاً اعمال شده‌اند
    }

    /// <summary>
    /// پردازش رویداد حذف مجوز
    /// </summary>
    private void On(RolePermissionRemovedEvent @event)
    {
        // معمولاً نیازی به تغییر حالت اینجا نیست
    }

    /// <summary>
    /// پردازش رویداد افزودن کاربر به نقش
    /// </summary>
    private void On(UserRoleAddedEvent @event)
    {
        // معمولاً نیازی به تغییر حالت اینجا نیست
    }

    /// <summary>
    /// پردازش رویداد حذف کاربر از نقش
    /// </summary>
    private void On(UserRoleRemovedEvent @event)
    {
        // معمولاً نیازی به تغییر حالت اینجا نیست
    }

    #endregion
}