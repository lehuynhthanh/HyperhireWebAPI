using System.Data;

namespace HyperhireWebAPI.Domain.Repositories;

public interface IUnitOfWork
{
    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    IDisposable BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, string? lockName = null);

    Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, string? lockName = null, CancellationToken cancellationToken = default);

    void CommitTransaction();

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}
