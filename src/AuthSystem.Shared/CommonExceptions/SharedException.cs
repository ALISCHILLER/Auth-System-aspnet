using System;

namespace AuthSystem.Shared.CommonExceptions;

public abstract class SharedException : Exception
{
    protected SharedException(string message, string errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public string ErrorCode { get; }
}