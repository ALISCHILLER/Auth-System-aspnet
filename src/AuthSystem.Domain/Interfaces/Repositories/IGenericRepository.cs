using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Interfaces.Repositories;

/// <summary>
/// رابط پایه برای تمام Repositoryها
/// این رابط عملیات پایه CRUD را برای تمام موجودیت‌ها تعریف می‌کند
/// </summary>
/// <typeparam name="T">نوع موجودیت</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// دریافت یک موجودیت بر اساس شناسه
    /// </summary>
    /// <param name="id">شناسه موجودیت</param>
    /// <returns>موجودیت یا null در صورت عدم وجود</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// دریافت یک موجودیت بر اساس شرط
    /// </summary>
    /// <param name="predicate">شرط فیلتر</param>
    /// <returns>موجودیت یا null در صورت عدم وجود</returns>
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// دریافت لیستی از موجودیت‌ها
    /// </summary>
    /// <returns>لیست موجودیت‌ها</returns>
    Task<IQueryable<T>> GetAllAsync();

    /// <summary>
    /// دریافت لیستی از موجودیت‌ها بر اساس شرط
    /// </summary>
    /// <param name="predicate">شرط فیلتر</param>
    /// <returns>لیست موجودیت‌ها</returns>
    Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// افزودن یک موجودیت جدید
    /// </summary>
    /// <param name="entity">موجودیت</param>
    Task AddAsync(T entity);

    /// <summary>
    /// افزودن لیستی از موجودیت‌ها
    /// </summary>
    /// <param name="entities">لیست موجودیت‌ها</param>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// به‌روزرسانی یک موجودیت
    /// </summary>
    /// <param name="entity">موجودیت</param>
    void Update(T entity);

    /// <summary>
    /// به‌روزرسانی لیستی از موجودیت‌ها
    /// </summary>
    /// <param name="entities">لیست موجودیت‌ها</param>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// حذف یک موجودیت
    /// </summary>
    /// <param name="entity">موجودیت</param>
    void Delete(T entity);

    /// <summary>
    /// حذف لیستی از موجودیت‌ها
    /// </summary>
    /// <param name="entities">لیست موجودیت‌ها</param>
    void DeleteRange(IEnumerable<T> entities);

    /// <summary>
    /// بررسی وجود موجودیت بر اساس شرط
    /// </summary>
    /// <param name="predicate">شرط فیلتر</param>
    /// <returns>در صورت وجود true باز می‌گرداند</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}