using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// روش‌های ارسال کد
/// </summary>
[Flags]
public enum DeliveryMethod
{
    /// <summary>
    /// ارسال از طریق ایمیل
    /// </summary>
    Email = 1,

    /// <summary>
    /// ارسال از طریق پیامک
    /// </summary>
    SMS = 2,

    /// <summary>
    /// نمایش در اپلیکیشن
    /// </summary>
    App = 4,

    /// <summary>
    /// تماس صوتی
    /// </summary>
    Voice = 8,

    /// <summary>
    /// پیام‌رسان (WhatsApp, Telegram, etc.)
    /// </summary>
    Messenger = 16
}
