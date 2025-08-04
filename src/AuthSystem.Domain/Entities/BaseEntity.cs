using AuthSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// کلاس پایه برای تمام موجودیت‌های دامنه
/// این کلاس شامل ویژگی‌ها و رفتارهای مشترک تمام موجودیت‌ها می‌شود
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// شناسه یکتای موجودیت (Primary Key)
    /// از نوع Guid برای اطمینان از یکتایی در سیستم‌های توزیع‌شده
    /// مقدار پیش‌فرض به صورت خودکار تولید می‌شود
    /// </summary>
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// تاریخ و زمان ایجاد موجودیت
    /// مقدار پیش‌فرض برابر با زمان فعلی سیستم است
    /// از زمان جهانی (UTC) استفاده می‌شود تا از مشکلات مربوط به منطقه زمانی جلوگیری شود
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// تاریخ و زمان آخرین به‌روزرسانی موجودیت
    /// Nullable است زیرا در لحظه ایجاد موجودیت، مقدار ندارد
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// لیست رویدادهای دامنه مرتبط با این موجودیت
    /// این فیلد برای پیاده‌سازی الگوی Domain Events استفاده می‌شود
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// دسترسی فقط-خواندنی به رویدادهای دامنه
    /// این ویژگی برای مشاهده رویدادها از خارج کلاس استفاده می‌شود
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// علامت‌گذاری موجودیت به عنوان به‌روز شده
    /// این متد زمان UpdatedAt را به زمان فعلی به‌روز می‌کند
    /// </summary>
    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// افزودن رویداد دامنه جدید به لیست رویدادها
    /// </summary>
    /// <param name="domainEvent">رویداد دامنه که باید اضافه شود</param>
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// پاک کردن تمام رویدادهای دامنه
    /// معمولاً بعد از پردازش رویدادها فراخوانی می‌شود
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// بررسی برابری دو موجودیت بر اساس شناسه
    /// در موجودیت‌ها، برابری بر اساس شناسه (Id) انجام می‌شود نه مرجع شیء
    /// </summary>
    public override bool Equals(object? obj)
    {
        // اگر شیء مقایسه شونده null است یا از نوع BaseEntity نیست
        if (obj is not BaseEntity other)
            return false;
        // اگر هر دو شیء به یک مرجع در حافظه اشاره می‌کنند
        if (ReferenceEquals(this, other))
            return true;
        // اگر نوع دو شیء متفاوت است
        if (GetType() != other.GetType())
            return false;
        // مقایسه بر اساس شناسه
        return Id == other.Id;
    }

    /// <summary>
    /// محاسبه هش کد برای موجودیت
    /// این متد برای استفاده در ساختارهای داده‌ای مانند Dictionary مهم است
    /// </summary>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    /// <summary>
    /// عملگر تساوی برای مقایسه دو موجودیت
    /// </summary>
    public static bool operator ==(BaseEntity? a, BaseEntity? b)
    {
        // اگر هر دو null هستند
        if (a is null && b is null)
            return true;
        // اگر یکی null و دیگری نه
        if (a is null || b is null)
            return false;
        // فراخوانی متد Equals
        return a.Equals(b);
    }

    /// <summary>
    /// عملگر عدم تساوی برای مقایسه دو موجودیت
    /// </summary>
    public static bool operator !=(BaseEntity? a, BaseEntity? b) => !(a == b);

    /// <summary>
    /// متد کمکی برای تنظیم شناسه
    /// فقط برای موارد خاص مثل بارگذاری از دیتابیس استفاده شود
    /// internal به EF Core اجازه می‌دهد از این متد استفاده کند
    /// </summary>
    /// <param name="id">شناسه جدید</param>
    internal void SetId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("شناسه نمی‌تواند خالی باشد", nameof(id));

        Id = id;
    }

    /// <summary>
    /// متد کمکی برای تنظیم تاریخ ایجاد
    /// فقط برای موارد خاص مثل بارگذاری از دیتابیس استفاده شود
    /// internal به EF Core اجازه می‌دهد از این متد استفاده کند
    /// </summary>
    /// <param name="createdAt">تاریخ ایجاد</param>
    internal void SetCreatedAt(DateTime createdAt)
    {
        if (createdAt.Kind != DateTimeKind.Utc)
            throw new ArgumentException("تاریخ باید از نوع UTC باشد", nameof(createdAt));

        CreatedAt = createdAt;
    }
}