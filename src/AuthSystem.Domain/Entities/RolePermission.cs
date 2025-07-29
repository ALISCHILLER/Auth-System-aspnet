namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت رابطه چند به چند بین نقش و مجوز
/// این کلاس برای مدیریت مجوزهای مربوط به هر نقش استفاده می‌شود
/// </summary>
public class RolePermission
{
    /// <summary>
    /// شناسه نقش
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// شناسه مجوز
    /// </summary>
    public Guid PermissionId { get; set; }

    // ویژگی‌های ناوبری (Navigation Properties)

    /// <summary>
    /// نقش مربوطه
    /// </summary>
    public Role Role { get; set; } = default!;

    /// <summary>
    /// مجوز مربوطه
    /// </summary>
    public Permission Permission { get; set; } = default!;
}