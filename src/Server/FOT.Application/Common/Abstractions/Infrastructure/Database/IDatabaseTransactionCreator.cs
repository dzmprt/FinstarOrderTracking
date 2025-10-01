namespace FOT.Application.Common.Abstractions.Persistence;

/// <summary>
/// Database transaction creator.
/// </summary>
public interface IDatabaseTransactionCreator
{
    /// <summary>
    /// Begin transaction.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns><see cref="IDatabaseTransaction"/>.</returns>
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}