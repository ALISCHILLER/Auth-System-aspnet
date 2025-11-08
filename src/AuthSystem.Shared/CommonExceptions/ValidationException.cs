using System;
using System.Collections.Generic;

namespace AuthSystem.Shared.CommonExceptions;

public sealed class ValidationException : SharedException
{
    public ValidationException(string message, IReadOnlyDictionary<string, string[]> errors)
        : base(message, "validation_error")
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}