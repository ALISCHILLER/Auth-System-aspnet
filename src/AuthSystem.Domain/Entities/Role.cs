using AuthSystem.Domain.Common;
using AuthSystem.Domain.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت نقش در سیستم احراز هویت
/// این کلاس نقش‌های مختلف کاربران را تعریف می‌کند (مثل Admin، User، Manager)
/// </summary>
public class Role : BaseEntity
{
    // Private fields
    private string _name;
    private string _description;

    // Encapsulated properties with only getters
    public string Name => _name;
    public string Description => _description;

    // Required for EF Core
    private Role() { }

    // Static factory method for creation
    public static Role Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        var role = new Role
        {
            _name = name.Trim(),
            _description = description?.Trim() ?? string.Empty
        };

        return role;
    }

    // Optional: Update methods (for aggregate root behavior)
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("New name cannot be empty.", nameof(newName));

        _name = newName.Trim();
    }

    public void UpdateDescription(string newDescription)
    {
        _description = newDescription?.Trim() ?? string.Empty;
    }
}