using AuthSystem.Domain.Common.Auditing;
using AuthSystem.Domain.Common.Clock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Base;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IAuditableEntity, ISoftDeletableEntity
    where TId : notnull
{
    private int? _requestedHashCode;
    protected Entity()
    {
        CreatedAt = DomainClock.Instance.UtcNow;
    }
    protected Entity(TId id)
        : this()
    {
        Id = id;
    }

    public TId Id { get; protected set; } = default!;


    public DateTime CreatedAt { get; protected set; }


    public DateTime? UpdatedAt { get; protected set; }


    public Guid? CreatedBy { get; protected set; }


    public Guid? UpdatedBy { get; protected set; }


    public bool IsDeleted { get; protected set; }


    public DateTime? DeletedAt { get; protected set; }

    public Guid? DeletedBy { get; protected set; }

    public bool IsTransient() => EqualityComparer<TId>.Default.Equals(Id, default!);

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        if (IsTransient() || other.IsTransient())
        {
            return false;
        }
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }
    public bool Equals(Entity<TId>? other) => Equals((object?)other);

    public override int GetHashCode()
    {
        if (IsTransient())
        {
            return base.GetHashCode();
        }

        _requestedHashCode ??= HashCode.Combine(Id, 31);
        return _requestedHashCode.Value;
    }

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);

    protected virtual void MarkAsCreated(Guid? createdBy = null, DateTime? occurredOn = null)
    {
        var timestamp = NormalizeTimestamp(occurredOn);
        CreatedAt = timestamp;
        CreatedBy = createdBy;
        UpdatedAt = timestamp;
        UpdatedBy = createdBy;
    }

    protected virtual void MarkAsUpdated(Guid? updatedBy = null, DateTime? occurredOn = null)
    {
        var timestamp = NormalizeTimestamp(occurredOn);
        UpdatedAt = timestamp;
        UpdatedBy = updatedBy;
    }

    protected virtual void MarkAsDeleted(Guid? deletedBy = null, DateTime? occurredOn = null)
    {
        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;
        DeletedAt = NormalizeTimestamp(occurredOn);
        DeletedBy = deletedBy;
        MarkAsUpdated(deletedBy, occurredOn);
    }

    protected virtual void ClearDeletion(Guid? restoredBy = null, DateTime? occurredOn = null)
    {
        if (!IsDeleted)
        {
            return;
        }

        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        MarkAsUpdated(restoredBy, occurredOn);
    }

    private static DateTime NormalizeTimestamp(DateTime? timestamp)
    {
        if (!timestamp.HasValue)
        {
            return DomainClock.Instance.UtcNow;
        }

        var value = timestamp.Value;
        return value.Kind switch
        {
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => value
        };
    }

    public override string ToString() => $"{GetType().Name} [Id={Id}]";
}