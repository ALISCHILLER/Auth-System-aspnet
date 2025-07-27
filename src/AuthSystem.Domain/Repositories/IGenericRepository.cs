using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace AuthSystem.Domain.Repositories;

/// <summary>
/// Interface پایه برای Repositoryهای Generic
/// </summary>
/// <typeparam name="T">نوع Entity</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// دریافت تمامی Entityها
    /// </summary>
    /// <returns>IQueryable برای اعمال فیلترها و مرتب‌سازی‌ها</returns>
    IQueryable<T> GetAll();

    /// <summary>
    /// پیدا کردن یک Entity بر اساس Id
    /// </summary>
    /// <param name="id">شناسه Entity</param>
    /// <returns>Entity یا null</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// پیدا کردن یک Entity بر اساس شرط
    /// </summary>
    /// <param name="predicate">شرط جستجو</param>
    /// <returns>Entity یا null</returns>
    Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// اضافه کردن یک Entity
    /// </summary>
    /// <param name="entity">Entity برای اضافه شدن</param>
    /// <returns>Task</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// بروزرسانی یک Entity
    /// </summary>
    /// <param name="entity">Entity بروز شده</param>
    void Update(T entity);

    /// <summary>
    /// حذف یک Entity
    /// </summary>
    /// <param name="entity">Entity برای حذف</param>
    void Delete(T entity);

    /// <summary>
    /// ذخیره تغییرات (برای Unit of Work)
    /// </summary>
    /// <returns>تعداد Entityهای تغییر یافته</returns>
    Task<int> SaveChangesAsync();
}