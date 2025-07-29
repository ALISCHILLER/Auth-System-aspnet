namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت رابطه چند به چند بین کاربر و نقش
/// این کلاس برای مدیریت روابط کاربران با نقش‌های مختلف استفاده می‌شود
/// </summary>
public class UserRole
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// شناسه نقش
    /// </summary>
    public Guid RoleId { get; set; }

    // ویژگی‌های ناوبری (Navigation Properties)

    /// <summary>
    /// کاربر مربوطه
    /// </summary>
    public User User { get; set; } = default!;

    /// <summary>
    /// نقش مربوطه
    /// </summary>
    public Role Role { get; set; } = default!;
}