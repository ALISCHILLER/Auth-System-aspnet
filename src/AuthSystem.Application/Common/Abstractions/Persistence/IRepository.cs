namespace AuthSystem.Application.Common.Abstractions.Persistence;

/// <summary>
/// ریپازیتوری عمومی برای موجودیت‌های دامنه
/// </summary>
/// <typeparam name="TEntity">نوع موجودیت</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Remove(TEntity entity);
}
