using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// کلاس پایهٔ تمام استثناهای لایهٔ Application.
/// - دارای کد خطا (Code) و پیام (Message).
/// - می‌تواند حاوی یک یا چند <see cref="Error"/> باشد.
/// - این کلاس برای نگاشت یکنواخت خطاها به لایهٔ API و لاگینگ طراحی شده است.
/// </summary>
public abstract class ApplicationException : Exception
{
    /// <summary>کد یکتای خطا برای نگاشت/لاگ/ترجمه.</summary>
    public string Code { get; }

    /// <summary>
    /// مجموعهٔ ساختاریافتهٔ خطاهای مرتبط با این استثنا.
    /// همیشه غیر‌تهی و Immutable است.
    /// </summary>
    public virtual IReadOnlyCollection<Error> Errors => _errors;
    private readonly IReadOnlyCollection<Error> _errors;

    /// <summary>
    /// سازندهٔ پایه با کد و پیام.
    /// در صورتی که مجموعهٔ خطاها ارائه نشود، یک Error از روی code/message ساخته می‌شود و ذخیره می‌گردد.
    /// </summary>
    protected ApplicationException(string code, string message)
        : base(message ?? throw new ArgumentNullException(nameof(message)))
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
        Code = code;
        _errors = new ReadOnlyCollection<Error>(new List<Error> { Error.Create(Code, Message) });
    }

    /// <summary>
    /// سازنده با کد، پیام و مجموعه‌ای از Error (غیر null).
    /// </summary>
    protected ApplicationException(string code, string message, IEnumerable<Error> errors)
        : base(message ?? throw new ArgumentNullException(nameof(message)))
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
        if (errors == null) throw new ArgumentNullException(nameof(errors));

        Code = code;

        var list = errors.ToList();
        if (list.Count == 0)
        {
            // تضمین حداقل یک Error تا middleware/response سازگار بماند
            list.Add(Error.Create(Code, Message));
        }

        _errors = new ReadOnlyCollection<Error>(list);
    }

    /// <summary>
    /// سازنده با کد، پیام، استثنای داخلی و مجموعه‌ای از Error (اختیاری).
    /// </summary>
    protected ApplicationException(string code, string message, Exception innerException, IEnumerable<Error>? errors = null)
        : base(message ?? throw new ArgumentNullException(nameof(message)), innerException)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

        Code = code;

        var list = (errors ?? new[] { Error.Create(Code, Message) }).ToList();
        if (list.Count == 0)
        {
            list.Add(Error.Create(Code, Message));
        }

        _errors = new ReadOnlyCollection<Error>(list);
    }
}