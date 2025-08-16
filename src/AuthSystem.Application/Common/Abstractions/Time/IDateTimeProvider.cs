namespace AuthSystem.Application.Common.Abstractions.Time;

/// <summary>
/// سرویس تامین زمان سیستم
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
