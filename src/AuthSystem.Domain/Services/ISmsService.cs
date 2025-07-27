using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Services
{
    /// <summary>
    /// Interface برای سرویس ارسال پیامک
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// ارسال پیامک
        /// </summary>
        /// <param name="to">شماره گیرنده</param>
        /// <param name="message">محتوای پیامک</param>
        /// <returns>Task</returns>
        Task SendSmsAsync(string to, string message);
    }
}