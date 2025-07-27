using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Services
{
    /// <summary>
    /// Interface برای سرویس تولید توکن‌ها
    /// </summary>
    public interface ITokenGenerator
    {
        /// <summary>
        /// تولید یک رشته توکن منحصر به فرد
        /// </summary>
        /// <returns>رشته توکن</returns>
        string GenerateToken();
    }
}