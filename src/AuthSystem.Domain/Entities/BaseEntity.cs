using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Entities
{
   /// <summary>
/// کلاس پایه برای تمامی Entityها
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// شناسه یکتای Entity
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// تاریخ ایجاد
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// تاریخ آخرین بروزرسانی
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
}