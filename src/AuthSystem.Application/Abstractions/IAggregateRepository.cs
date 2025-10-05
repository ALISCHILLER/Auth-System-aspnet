namespace AuthSystem.Application.Abstractions;

public interface IAggregateRepository<TAggregate> where TAggregate : class
{
    Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(TAggregate aggregate, CancellationToken ct);
    Task UpdateAsync(TAggregate aggregate, CancellationToken ct);
    Task DeleteAsync(TAggregate aggregate, CancellationToken ct);
}