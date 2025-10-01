
using System;
using System.Threading;

namespace AuthSystem.Domain.Common.Clock;

/// <summary>
/// Default domain clock that enforces UTC-first semantics and allows overriding for tests.
/// </summary>
public sealed class DomainClock : ISystemClock
{
    private static readonly Lazy<DomainClock> InstanceFactory = new(() => new DomainClock());
    private readonly ReaderWriterLockSlim _lock = new();
    private Func<DateTime> _utcNowProvider = () => DateTime.UtcNow;

    private DomainClock()
    {
    }

    /// <summary>
    /// Gets the singleton instance of the domain clock.
    /// </summary>
    public static DomainClock Instance => InstanceFactory.Value;

    /// <summary>
    /// Gets the current UTC time provided by the clock.
    /// </summary>
    public DateTime UtcNow
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                var value = _utcNowProvider();
                return value.Kind == DateTimeKind.Utc
                    ? value
                    : DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    /// <summary>
    /// Overrides the current clock with a custom implementation.
    /// </summary>
    public void Use(ISystemClock clock)
    {
        if (clock is null) throw new ArgumentNullException(nameof(clock));

        _lock.EnterWriteLock();
        try
        {
            _utcNowProvider = () => clock.UtcNow;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Fixes the current time to a specific UTC value.
    /// </summary>
    public void SetFixedTime(DateTime utcNow)
    {
        if (utcNow.Kind == DateTimeKind.Unspecified)
        {
            utcNow = DateTime.SpecifyKind(utcNow, DateTimeKind.Utc);
        }
        else if (utcNow.Kind == DateTimeKind.Local)
        {
            utcNow = utcNow.ToUniversalTime();
        }

        var captured = utcNow;
        _lock.EnterWriteLock();
        try
        {
            _utcNowProvider = () => captured;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Advances the clock by the provided offset when fixed time is being used.
    /// </summary>
    public void Advance(TimeSpan offset)
    {
        _lock.EnterWriteLock();
        try
        {
            var currentProvider = _utcNowProvider;
            _utcNowProvider = () => currentProvider().Add(offset);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Resets the clock to the system UTC time provider.
    /// </summary>
    public void Reset()
    {
        _lock.EnterWriteLock();
        try
        {
            _utcNowProvider = () => DateTime.UtcNow;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}