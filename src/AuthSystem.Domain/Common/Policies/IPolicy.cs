// File: AuthSystem.Domain/Common/Policies/IPolicy.cs
namespace AuthSystem.Domain.Common.Policies
{
    /// <summary>
    /// قرارداد Policy همگام
    /// - تصمیم‌گیری «اجازه/عدم‌اجازه» بر اساس زمینهٔ ورودی
    /// </summary>
    public interface IPolicy<in TContext>
    {
        /// <summary>ارزیابی سیاست و بازگرداندن نتیجه</summary>
        PolicyResult Evaluate(TContext context);
    }
}