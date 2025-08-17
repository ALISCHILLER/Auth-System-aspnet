using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// فرمت کد تأیید
/// </summary>
public enum CodeFormat
{
    /// <summary>
    /// فقط عددی
    /// </summary>
    Numeric,

    /// <summary>
    /// ترکیب حروف و اعداد
    /// </summary>
    AlphaNumeric
}
