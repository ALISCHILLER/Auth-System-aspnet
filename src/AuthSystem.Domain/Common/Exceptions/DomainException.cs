using System;
using AuthSystem.Domain.Common.Errors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthSystem.Domain.Common.Exceptions;


public abstract class DomainException : Exception
{

    protected DomainException(string message)
       : base(message)
    {
    }

    protected DomainException(string message, Exception innerException)
       : base(message, innerException)
    {
    }

    protected DomainException(DomainError error)
      : base(error.Message)
    {
        Error = error;
    }
    public virtual string ErrorCode => GetType().Name;

    public DomainError? Error { get; }
}