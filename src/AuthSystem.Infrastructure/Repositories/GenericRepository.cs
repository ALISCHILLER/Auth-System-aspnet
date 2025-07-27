using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Domain.Repositories;
using AuthSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthSystem.Infrastructure.Repositories;

/// <summary>
/// پیاده‌سازی Generic Repository
/// </summary>
/// <typeparam name="T">نوع Entity</typeparam>
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
    /// دریافت تمامی Entityها
    /// </summary>
    public IQueryable<T> GetAll()
    {
        return _dbSet.AsNoTracking(); // برای خواندن بهتر عملکرد دارد
    }

    /// <summary>
    /// پیدا کردن یک Entity بر اساس Id
    /// </summary>
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// پیدا کردن یک Entity بر اساس شرط
    /// </summary>
    public async Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// اضافه کردن یک Entity
    /// </summary>
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    /// <summary>
    /// بروزرسانی یک Entity
    /// </summary>
    public void Update(T entity)
    {
        _dbSet.Update(entity); // EF Core خودش تشخیص می‌ده که چی بروز بشه
    }

    /// <summary>
    /// حذف یک Entity
    /// </summary>
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// ذخیره تغییرات
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}