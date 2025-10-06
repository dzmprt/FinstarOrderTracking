using FOT.Application.Common.Abstractions.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace FOT.DatabaseInfrastructure.Implementations;

/// <inheritdoc />
public class DatabaseTransaction(IDbContextTransaction contextTransaction) : IDatabaseTransaction
{
    /// <inheritdoc />
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await contextTransaction.CommitAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await contextTransaction.RollbackAsync(cancellationToken);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        contextTransaction.Dispose();
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        return contextTransaction.DisposeAsync();
    }
}