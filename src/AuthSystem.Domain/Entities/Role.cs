using AuthSystem.Domain.Common;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت نقش در سیستم احراز هویت.
/// این کلاس نقش‌های مختلف کاربران را تعریف می‌کند (مثل Admin، User، Manager)
/// </summary>
public class Role : BaseEntity
{
    #region Fields

    /// <summary>
    /// نام نقش
    /// </summary>
    private string _name;

    /// <summary>
    /// توضیحات نقش
    /// </summary>
    private string _description;

    #endregion

    #region Properties

    /// <summary>
    /// خواندن نام نقش
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// خواندن توضیحات نقش
    /// </summary>
    public string Description => _description;

    #endregion

    #region Navigation Properties

    /// <summary>
    /// لیست روابط نقش با کاربران (برای رابطه چند به چند)
    /// </summary>
    private readonly List<UserRole> _userRoles = new();

    /// <summary>
    /// دسترسی فقط خواندنی به روابط نقش با کاربران
    /// </summary>
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    /// <summary>
    /// لیست روابط نقش با مجوزها (برای رابطه چند به چند)
    /// </summary>
    private readonly List<RolePermission> _rolePermissions = new();

    /// <summary>
    /// دسترسی فقط خواندنی به روابط نقش با مجوزها
    /// </summary>
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    #endregion

    #region Constructors

    /// <summary>
    /// سازنده پیش‌فرض برای EF Core
    /// </summary>
    private Role() { }

    #endregion

    #region Static Factory Method

    /// <summary>
    /// ساخت نمونه جدید از نقش با بررسی‌های اولیه و مقداردهی پیش‌فرض
    /// </summary>
    /// <param name="name">نام نقش</param>
    /// <param name="description">توضیحات نقش (اختیاری)</param>
    /// <returns>نمونه‌ای از نقش</returns>
    public static Role Create(string name, string description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidRoleNameException("نام نقش نمی‌تواند خالی باشد");

        var role = new Role
        {
            _name = name.Trim(),
            _description = description?.Trim() ?? string.Empty
        };

        // مقداردهی اولیه موجودیت (ID، تاریخ‌ها، وضعیت‌ها)
        role.InitializeEntity();

        return role;
    }

    #endregion

    #region Update Methods

    /// <summary>
    /// به‌روزرسانی نام نقش
    /// </summary>
    /// <param name="newName">نام جدید</param>
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new InvalidRoleNameException("نام جدید نقش نمی‌تواند خالی باشد");

        _name = newName.Trim();
        MarkAsUpdated();
    }

    /// <summary>
    /// به‌روزرسانی توضیحات نقش
    /// </summary>
    /// <param name="newDescription">توضیحات جدید</param>
    public void UpdateDescription(string newDescription)
    {
        _description = newDescription?.Trim() ?? string.Empty;
        MarkAsUpdated();
    }

    #endregion

    #region Relationship Management

    /// <summary>
    /// افزودن یک رابطه کاربر-نقش
    /// </summary>
    /// <param name="userRole">رابطه کاربر-نقش</param>
    internal void AddUserRole(UserRole userRole)
    {
        if (!_userRoles.Contains(userRole))
        {
            _userRoles.Add(userRole);
        }
    }

    /// <summary>
    /// حذف یک رابطه کاربر-نقش
    /// </summary>
    /// <param name="userRole">رابطه کاربر-نقش</param>
    internal void RemoveUserRole(UserRole userRole)
    {
        _userRoles.Remove(userRole);
    }

    /// <summary>
    /// افزودن یک رابطه نقش-مجوز
    /// </summary>
    /// <param name="rolePermission">رابطه نقش-مجوز</param>
    internal void AddRolePermission(RolePermission rolePermission)
    {
        if (!_rolePermissions.Contains(rolePermission))
        {
            _rolePermissions.Add(rolePermission);
        }
    }

    /// <summary>
    /// حذف یک رابطه نقش-مجوز
    /// </summary>
    /// <param name="rolePermission">رابطه نقش-مجوز</param>
    internal void RemoveRolePermission(RolePermission rolePermission)
    {
        _rolePermissions.Remove(rolePermission);
    }

    #endregion
}
