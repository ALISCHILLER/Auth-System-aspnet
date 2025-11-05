using System;

namespace AuthSystem.Application.Common.Abstractions.Time;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}