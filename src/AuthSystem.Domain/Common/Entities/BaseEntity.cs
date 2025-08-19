using System;
using System.Collections.Generic;
using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.Common.Entities;

/// <summary>
/// کلاس پایه برای تمام موجودیت‌های دامنه (Entity)
/// - دارای شناسه، زمان‌های ایجاد/به‌روزرسانی، و تساوی مبتنی بر Id
/// - استفاده از DomainClock برای تست‌پذیری و جداسازی از زمان سیستم
/// - auditing کامل با CreatedBy/UpdatedBy
/// - soft-delete با IsDeleted/DeletedAt
/// </summary>
public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
    where TId : notnull
{
    private int? _requestedHashCode;

    /// <summary>
    /// شناسه یکتای موجودیت
    /// </summary>
    public TId Id { get; protected set; } = default!;

    /// <summary>
    /// تاریخ ایجاد موجودیت (UTC)
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// تاریخ آخرین به‌روزرسانی (UTC)
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// شناسه کاربر ایجاد کننده
    /// </summary>
    public Guid? CreatedBy { get; protected set; }

    /// <summary>
    /// شناسه کاربر ویرایش کننده
    /// </summary>
    public Guid? UpdatedBy { get; protected set; }

    /// <summary>
    /// نشانه‌گذاری حذف منطقی
    /// </summary>
    public bool IsDeleted { get; protected set; }

    /// <summary>
    /// تاریخ حذف (در صورت حذف منطقی)
    /// </summary>
    public DateTime? DeletedAt { get; protected set; }

    /// <summary>
    /// سازنده پیش‌فرض: ایجاد زمان ایجاد به صورت UTC از طریق DomainClock
    /// - استفاده از DomainClock به جای DateTime.UtcNow برای تست‌پذیری
    /// </summary>
    protected BaseEntity()
    {
        CreatedAt = DomainClock.Instance.UtcNow;
    }

    /// <summary>
    /// سازنده با شناسه
    /// </summary>
    protected BaseEntity(TId id) : this()
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id), "شناسه موجودیت نمی‌تواند null باشد");

        Id = id;
    }

    /// <summary>
    /// آیا موجودیت هنوز موقتی است (در پایگاه داده ذخیره نشده)
    /// </summary>
    public bool IsTransient() => EqualityComparer<TId>.Default.Equals(Id, default!);

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        if (IsTransient() || other.IsTransient()) return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <inheritdoc />
    public bool Equals(BaseEntity<TId>? other) => Equals((object?)other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        if (IsTransient()) return base.GetHashCode();
        if (!_requestedHashCode.HasValue)
            _requestedHashCode = Id.GetHashCode() ^ 31;
        return _requestedHashCode.Value;
    }

    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right) =>
        !(left == right);

    /// <summary>
    /// علامت‌گذاری موجودیت به‌عنوان «به‌روزرسانی شده»
    /// - استفاده از DomainClock برای ثبت زمان به‌روزرسانی
    /// - متدهای تغییردهنده وضعیت باید این متد را فراخوانی کنند
    /// </summary>
    protected virtual void MarkAsUpdated(Guid? updatedBy = null)
    {
        UpdatedAt = DomainClock.Instance.UtcNow;
        UpdatedBy = updatedBy;
    }

    /// <summary>
    /// علامت‌گذاری موجودیت به‌عنوان «حذف شده»
    /// - استفاده از soft-delete برای حفظ تاریخچه
    /// </summary>
    protected virtual void MarkAsDeleted(Guid? deletedBy = null)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        DeletedAt = DomainClock.Instance.UtcNow;
        MarkAsUpdated(deletedBy);
    }

    /// <inheritdoc />
    public override string ToString() => $"{GetType().Name} [Id={Id}]";
}