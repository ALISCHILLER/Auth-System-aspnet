using System;
using System.Collections.Generic;

namespace AuthSystem.Domain.Common;

/// <summary>
/// کلاس پایه برای تمام موجودیت‌های دامنه
/// این کلاس هویت، برابری و قابلیت‌های پایه را برای موجودیت‌ها فراهم می‌کند
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
    /// سازنده پیش‌فرض
    /// </summary>
    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
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
    /// بررسی موجودیت موقت (هنوز در دیتابیس ذخیره نشده)
    /// </summary>
    public bool IsTransient()
    {
        return EqualityComparer<TId>.Default.Equals(Id, default);
    }

    /// <summary>
    /// مقایسه برابری دو موجودیت
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TId> entity)
            return false;

        if (ReferenceEquals(this, entity))
            return true;

        if (GetType() != entity.GetType())
            return false;

        if (entity.IsTransient() || IsTransient())
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, entity.Id);
    }

    /// <summary>
    /// مقایسه برابری نوع‌دار
    /// </summary>
    public bool Equals(BaseEntity<TId>? other)
    {
        return Equals((object?)other);
    }

    /// <summary>
    /// محاسبه هش کد بر اساس شناسه
    /// </summary>
    public override int GetHashCode()
    {
        if (IsTransient())
            return base.GetHashCode();

        if (!_requestedHashCode.HasValue)
            _requestedHashCode = Id.GetHashCode() ^ 31;

        return _requestedHashCode.Value;
    }

    /// <summary>
    /// عملگر برابری
    /// </summary>
    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    /// <summary>
    /// عملگر نابرابری
    /// </summary>
    public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// به‌روزرسانی زمان آخرین تغییر
    /// این متد باید در متدهایی که موجودیت را تغییر می‌دهند فراخوانی شود
    /// </summary>
    protected virtual void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// بازنویسی ToString برای نمایش بهتر در دیباگ
    /// </summary>
    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }
}
