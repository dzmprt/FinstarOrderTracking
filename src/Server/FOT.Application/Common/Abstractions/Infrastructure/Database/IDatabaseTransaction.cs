namespace FOT.Application.Common.Abstractions.Infrastructure.Database;

/// <summary>
/// Database transaction.
/// </summary>
public interface IDatabaseTransaction : IDisposable
{
    /// <summary>
    /// Commit all changes and close transaction.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Task of operation.</returns>
    Task CommitAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Rollback all changed and close transaction. 
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Task of operation.</returns>
    Task RollbackAsync(CancellationToken cancellationToken);
}