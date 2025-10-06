using FOT.Application.Common.Abstractions.Infrastructure.Database;

namespace FOT.DatabaseInfrastructure.Implementations;

/// <inheritdoc />
public class DatabaseTransactionCreator(ApplicationDbContext dbContext) : IDatabaseTransactionCreator
{
    /// <inheritdoc />
    public async Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return new DatabaseTransaction(await dbContext.Database.BeginTransactionAsync(cancellationToken));
    }
}