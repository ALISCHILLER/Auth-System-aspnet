using AuthSystem.Domain.Interfaces.Repositories;
using AuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure.Persistence.Repositories;

/// <summary>
/// پیاده‌سازی عمومی GenericRepository برای تمام موجودیت‌ها
/// </summary>
/// <typeparam name="T">نوع موجودیت</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="context">DbContext</param>
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// دریافت یک موجودیت بر اساس شناسه
    /// </summary>
    /// <param name="id">شناسه موجودیت</param>
    /// <returns>موجودیت یا null در صورت عدم وجود</returns>
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// دریافت یک موجودیت بر اساس شرط
    /// </summary>
    /// <param name="predicate">شرط فیلتر</param>
    /// <returns>موجودیت یا null در صورت عدم وجود</returns>
    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// دریافت لیستی از موجودیت‌ها
    /// </summary>
    /// <returns>لیست موجودیت‌ها</returns>
    public async Task<IQueryable<T>> GetAllAsync()
    {
        return _dbSet.AsQueryable();
    }

    /// <summary>
    /// دریافت لیستی از موجودیت‌ها بر اساس شرط
    /// </summary>
    /// <param name="predicate">شرط فیلتر</param>
    /// <returns>لیست موجودیت‌ها</returns>
    public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    /// <summary>
    /// افزودن یک موجودیت جدید
    /// </summary>
    /// <param name="entity">موجودیت</param>
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    /// <summary>
    /// افزودن لیستی از موجودیت‌ها
    /// </summary>
    /// <param name="entities">لیست موجودیت‌ها</param>
    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    /// <summary>
    /// به‌روزرسانی یک موجودیت
    /// </summary>
    /// <param name="entity">موجودیت</param>
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// به‌روزرسانی لیستی از موجودیت‌ها
    /// </summary>
    /// <param name="entities">لیست موجودیت‌ها</param>
    public void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    /// <summary>
    /// حذف یک موجودیت
    /// </summary>
    /// <param name="entity">موجودیت</param>
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// حذف لیستی از موجودیت‌ها
    /// </summary>
    /// <param name="entities">لیست موجودیت‌ها</param>
    public void DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    /// <summary>
    /// بررسی وجود موجودیت بر اساس شرط
    /// </summary>
    /// <param name="predicate">شرط فیلتر</param>
    /// <returns>در صورت وجود true باز می‌گرداند</returns>
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
}