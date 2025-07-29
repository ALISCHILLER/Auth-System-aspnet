using AuthSystem.Domain.Interfaces.Repositories;
using AuthSystem.Infrastructure.Persistence.Contexts;
using AuthSystem.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure.Persistence;

/// <summary>
/// پیاده‌سازی IUnitOfWork
/// این کلاس به کنترل تراکنش‌ها و ذخیره تغییرات کمک می‌کند
/// </summary>
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;
    private IUserRepository? _users;
    private IRoleRepository? _roles;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="context">DbContext</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// دسترسی به Repository کاربران
    /// </summary>
    public IUserRepository Users => _users ??= new UserRepository(_context);

    /// <summary>
    /// دسترسی به Repository نقش‌ها
    /// </summary>
    public IRoleRepository Roles => _roles ??= new RoleRepository(_context);

    /// <summary>
    /// ذخیره تغییرات در دیتابیس
    /// </summary>
    /// <returns>تعداد رکوردهای تغییر کرده</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// شروع یک تراکنش جدید
    /// </summary>
    /// <returns>تراکنش</returns>
    public async Task<IDisposable> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// آزادسازی منابع
    /// </summary>
    public void Dispose()
    {
        _context?.Dispose();
    }
}