using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Services
{
    /// <summary>
    /// Interface برای سرویس هش کردن رمز عبور
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// هش کردن یک رمز عبور
        /// </summary>
        /// <param name="password">رمز عبور متنی</param>
        /// <returns>رشته هش شده</returns>
        string HashPassword(string password);

        /// <summary>
        /// بررسی صحت یک رمز عبور در برابر هش آن
        /// </summary>
        /// <param name="password">رمز عبور متنی</param>
        /// <param name="hashedPassword">رشته هش شده</param>
        /// <returns>True اگر مطابقت داشته باشد</returns>
        bool VerifyPassword(string password, string hashedPassword);
    }
}