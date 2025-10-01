using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Domain.Common.Entities;

/// <summary>
/// Base class for immutable value objects with structural equality.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
  
    protected abstract IEnumerable<object?> GetEqualityComponents();

    
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }
        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

  
    public bool Equals(ValueObject? other) => Equals((object?)other);

    public override int GetHashCode()
    {
        return GetEqualityComponents()
              .Select(component => component?.GetHashCode() ?? 0)
              .Aggregate(0, HashCode.Combine);
    }

    public static bool operator ==(ValueObject? left, ValueObject? right) =>
       left is null && right is null || left is not null && left.Equals(right);

    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);

   
    public override string ToString()
    {
        var components = string.Join(", ", GetEqualityComponents().Select(x => x?.ToString() ?? "null"));
        return $"{GetType().Name}({components})";
    }
}