using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Services
{
    /// <summary>
    /// Interface برای ارائه زمان فعلی (مفید برای تست)
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// دریافت زمان UTC فعلی
        /// </summary>
        DateTime UtcNow { get; }
    }
}