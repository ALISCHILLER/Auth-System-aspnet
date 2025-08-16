using System;
using System.Collections.Generic;
using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// کلاس پایهٔ استثناهای دامنه (Domain).
/// این استثناها برای شرایطی استفاده می‌شوند که یک قانون دامنه نقض شده باشد.
/// توجه: اکثر خطاهای کسب‌وکار باید از طریق <see cref="Result{T}"/> مدیریت شوند.
/// این استثنا فقط برای موارد استثنایی یا در موقعیت‌هایی که از مسیر منطق بیزینس خارج می‌شویم استفاده می‌شود.
/// </summary>
public abstract class DomainException : ApplicationException
{
    protected DomainException(string code, string message, IEnumerable<Error>? errors = null)
        : base(code, message, errors ?? new[] { Error.Create(code, message) })
    {
    }

    protected DomainException(string code, string message, Exception innerException, IEnumerable<Error>? errors = null)
        : base(code, message, innerException, errors)
    {
    }
}