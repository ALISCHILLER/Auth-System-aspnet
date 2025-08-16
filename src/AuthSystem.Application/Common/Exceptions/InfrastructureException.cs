using System;
using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// کلاس پایهٔ استثناهای زیرساختی.
/// برای خطاهایی که از سیستم‌های خارجی، I/O، پایگاه داده یا پیکربندی ناشی می‌شوند.
/// </summary>
public abstract class InfrastructureException : ApplicationException
{
    protected InfrastructureException(string code, string message)
        : base(code, message, new[] { Error.Failure(message, code) })
    {
    }

    protected InfrastructureException(string code, string message, Exception innerException)
        : base(code, message, innerException, new[] { Error.Failure(message, code) })
    {
    }
}