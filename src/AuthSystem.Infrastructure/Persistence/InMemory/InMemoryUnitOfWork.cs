using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Infrastructure.Persistence.InMemory;

internal sealed class InMemoryUnitOfWork(ILogger<InMemoryUnitOfWork> logger) : IUnitOfWork
{
    private int _transactionDepth;

    public Task BeginAsync(CancellationToken ct)
    {
        Interlocked.Increment(ref _transactionDepth);
        logger.LogDebug("In-memory transaction started. Depth: {Depth}", _transactionDepth);
        return Task.CompletedTask;
    }

    public Task CommitAsync(CancellationToken ct)
    {
        if (_transactionDepth <= 0)
        {
            return Task.CompletedTask;
        }

        var depth = Interlocked.Decrement(ref _transactionDepth);
        logger.LogDebug("In-memory transaction committed. Remaining depth: {Depth}", depth);
        return Task.CompletedTask;
    }

    public Task RollbackAsync(CancellationToken ct)
    {
        if (_transactionDepth <= 0)
        {
            return Task.CompletedTask;
        }

        var depth = Interlocked.Decrement(ref _transactionDepth);
        logger.LogWarning("In-memory transaction rolled back. Remaining depth: {Depth}", depth);
        return Task.CompletedTask;
    }
}