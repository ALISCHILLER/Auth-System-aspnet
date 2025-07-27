using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Services
{
    /// <summary>
    /// Interface برای سرویس ارسال ایمیل
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// ارسال ایمیل
        /// </summary>
        /// <param name="to">آدرس گیرنده</param>
        /// <param name="subject">موضوع ایمیل</param>
        /// <param name="body">محتوای ایمیل</param>
        /// <returns>Task</returns>
        Task SendEmailAsync(string to, string subject, string body);
    }
}