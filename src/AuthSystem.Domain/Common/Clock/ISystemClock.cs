using System;

namespace AuthSystem.Domain.Common.Clock;

/// <summary>
/// Provides access to the current UTC time inside the domain layer.
/// </summary>
public interface ISystemClock
{
    /// <summary>
    /// Provides access to the current UTC time inside the domain layer.
    /// </summary>
    DateTime UtcNow { get; }
}